using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerObject : MobileObject {

	public float movementSpeed = 10.0f;
	public float lookSensitivity = 5.0f;
	[SerializeField]
	float diagonalResponsiveness = 0.05f;
	[SerializeField]
	float inertia = 0.981f;
	
	float gravity;
	float lookx;
	float momentumX = 0.0f;
	float momentumY = 0.0f;
	
	void Start(){

		this.Health = this.maxHealth;
		
		InvokeRepeating("RegenHealth", 1, 1.0f);
		Debug.Log("---Damage at end of start: " + this.targetDamage);
	}
	
	[RPC]
	public void PlayerSetup(int viewID, int senderID){
		
		if (senderID == PhotonNetwork.player.ID) {
			PhotonView photonView = PhotonView.Find(viewID);
			if (photonView == null){
				Debug.LogError("No PhotonView found with ID: " + viewID);
			}
			
			GameObject player = photonView.gameObject;
			
			PlayerHandler.EnableLocalControl(player);
			PlayerHandler.RegisterPlayerValues(player);
			PlayerHandler.SetCamera(player);
			
			GameObject.FindObjectOfType<GUIHandler>().enabled = true;
		}
	}

	void Move(){
		
		//TODO: This needs to send input information to the server as well
	
		float time = Time.deltaTime;
		float moveX = momentumX;
		float moveY = momentumY;
		if (this.controller.isGrounded && movementEnabled) {
			gravity = 0f;		
			moveX = Input.GetAxis("Horizontal") * movementSpeed * time;
			moveY = Input.GetAxis("Vertical") * movementSpeed * time;
			// Diagonal movement compensation
			if (Mathf.Abs(moveX) > diagonalResponsiveness && Mathf.Abs(moveY) > diagonalResponsiveness){
				float diagonal_move = Mathf.Sqrt(moveX * moveX + moveY * moveY);
				moveX = diagonal_move / 2 * Mathf.Sign(moveX);
				moveY = diagonal_move / 2 * Mathf.Sign(moveY);
			}
		} else {
			gravity -= 9.81f * time * time;
		}
		momentumX = moveX * inertia;
		momentumY = moveY * inertia;
		
		MoveObject(new Vector3(momentumX, gravity, momentumY));
	}
	
	public void Update(){

		Move();
		Actions.Update();
	}
	
	protected override void EndObject(){
	
		transform.position = GameObject.Find("SpawnPoint").transform.position;
		this.Health = this.maxHealth;
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
		//TODO: Add a hashtable with input state for authoritative movement/actions
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
	
	/// <summary>
	/// Loads passed item to passed location, removes item in existing location 
	/// if it exists or moves existing item to passed location.
	/// </summary>
	/// <param name="itemToLoad">Item to load.</param>
	/// <param name="targetLocation">Target location.</param>
	//HACK THIS DOESN"T GO HERE AT AAAAALLLL. Temporary!!!

	private void GetTargetInfo(){
		
		Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit targetInfo;
		if (Physics.Raycast(mouseRay, out targetInfo)){
			DestructableObject info = targetInfo.transform.GetComponent<DestructableObject>();
			if (info != null){
				string curHealth = string.Format("Health: {0}", info.Health);
				GUI.Box(new Rect(Screen.width/2-50, 30, 100, 20), curHealth);
				GUI.Box(new Rect(Screen.width/2-info.Health/2, 60, info.Health, 20), "");
			}
		}
	}
	
	void OnGUI(){

		if(PhotonNetwork.isMasterClient) {return;}
		string curHealth = string.Format("Health: {0}/{1}", this.Health, this.maxHealth);
		GUI.Box(new Rect(Screen.width/2-50, Screen.height-50, 100, 20), curHealth);
		GUI.Box(new Rect(Screen.width/2-this.Health/2, Screen.height-30, this.Health, 20), "");
		GetTargetInfo();
	}
}
