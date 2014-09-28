using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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
	}
	
	public void Move(float horizontal, float vertical){
		
		//TODO: This needs to send input information to the server as well
	
		float time = Time.fixedDeltaTime;
		float moveX = momentumX;
		float moveY = momentumY;
		if (this.controller.isGrounded) {
			gravity = 0f;		
			moveX = horizontal * movementSpeed * time;
			moveY = vertical * movementSpeed * time;
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

	public void MouseLook(float horizontal, float vertical) {

		Transform camera = transform.FindChild("MainCamera");

		if (camera == null) {
			Debug.LogError("NO CAMERA!");
		}

		float mouseLookX = horizontal * lookSensitivity;
		float mouseLookY = vertical * -lookSensitivity;
		//TODO: Make rotation happen on Master without a camera
		//camera.RotateAround(transform.localPosition, transform.right, mouseLookY);
		transform.Rotate(new Vector3(mouseLookY, mouseLookX, 0.0f));

		Debug.Log("Rotation on the server");
	}

	void RaiseMouseLookEvent() {

		if(!mouseLookEnabled) {return;}

		Transform camera = transform.FindChild("MainCamera");

		if (camera == null) {
			Debug.LogError("NO CAMERA!");
		}

		float mouseY = Input.GetAxis("Mouse Y");
		float mouseX = Input.GetAxis("Mouse X");

		var parameters = new Dictionary<int, object>();
		parameters.Add(GameEventParameter.Horizontal, mouseX);
		parameters.Add(GameEventParameter.Vertical, mouseY);
		parameters.Add(GameEventParameter.SenderViewID, gameObject.GetPhotonView().viewID);

		if(mouseX != 0f | mouseY != 0f) {

			//MouseLook(mouseX, mouseY);
			NetworkManager.RaiseEvent(GameEventCode.RotatePlayer, parameters, false);
		}
	}
	
	void RaiseMoveEvent(){
	
		if (!movementEnabled){ return; }

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		//Move (horizontal, vertical);
		
		var parameters = new Dictionary<int, object>();
		parameters.Add(GameEventParameter.Horizontal, horizontal);		
		parameters.Add(GameEventParameter.Vertical, vertical);
		parameters.Add(GameEventParameter.SenderViewID, gameObject.GetPhotonView().viewID);
		
		if (horizontal != 0f | vertical != 0f){
			NetworkManager.RaiseEvent(GameEventCode.MovePlayer, parameters, false);
		}
	}
	
	void FixedUpdate(){
	
		RaiseMoveEvent();	
		RaiseMouseLookEvent();
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
	
	#region RPCs
	
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
	
	[RPC]
	public void ToggleMovement(bool toggle, PhotonMessageInfo info){
	
		if (info.photonView.viewID == PhotonView.Get(this).viewID){
			GetComponent<PlayerObject>().movementEnabled = toggle;
		}
	}
	
	[RPC]
	public void ToggleLook(bool toggle, PhotonMessageInfo info){
		
		if (info.photonView.viewID == PhotonView.Get(this).viewID){
			GetComponent<PlayerObject>().mouseLookEnabled = toggle;
		}
	}
	
	[RPC]
	public void TogglePrimaryAction(bool toggle, PhotonMessageInfo info){
		
		if (info.photonView.viewID == PhotonView.Get(this).viewID){
			GetComponent<PlayerObject>().primaryActionEnabled = toggle;
		}
	}
	
	[RPC]
	public void ToggleActions(bool toggle, PhotonMessageInfo info){
		
		if (info.photonView.viewID == PhotonView.Get(this).viewID){
			GetComponent<PlayerObject>().actionsEnabled = toggle;
		}
	}
	
	#endregion
}
