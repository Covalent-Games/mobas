using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerObject : MobileObject {

	//TODO Move this the heck out of here. Should be handled on the server.
	public Dictionary<string, string> equipmentList = new Dictionary<string, string>();

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
	public bool mouseLookEnabled = false;
	
	void Start(){
		
		this.Health = this.maxHealth;
			
		name = gameObject.GetInstanceID().ToString();
		if(PhotonNetwork.isMasterClient) {
			gameObject.AddComponent("MasterRpcList");
		}
		
		InvokeRepeating("RegenHealth", 1, 1.0f);
	}
	
	void Move(){
	
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
	}
	
	protected override void CheckIfDestroyed(){
	
		if (this.Health <= 0){
			transform.position = GameObject.Find("SpawnPoint").transform.position;
			this.Health = this.maxHealth;
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
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
	[RPC]
	void SetItemLocation(string itemToLoad, string targetLocation){
		
		// Get the avatar's photonview to perform the call on
		PhotonView photonView = gameObject.GetComponent<PhotonView>();
		// Get their equipment "list" -- this will want to be stored on the server eventually
		Transform newItem;
		
		// If the location(value) is already occupied, remove the item(key) in it. 
		// TODO make this able to move the item to an unoccupied slot if possible instead of destroying it
		if (equipmentList.ContainsValue(targetLocation)){
			foreach (string item in equipmentList.Keys){
				if (equipmentList[item] == targetLocation){
					Destroy(photonView.transform.FindChild(targetLocation).FindChild(
						item+"(Clone)").gameObject);
					equipmentList.Remove(item);
					break;
				}
			}
		}
		// If we already have this item, reference the existing one instead of making another
		if (equipmentList.ContainsKey(itemToLoad)){
			newItem = photonView.transform.FindChild(equipmentList[itemToLoad]).FindChild(itemToLoad+"(Clone)");
			// Since we're moving the item we'll remove the entry in the avatars equipment list
			equipmentList.Remove(itemToLoad);
			// The weapon doesn't already exist so we create it
		}else{
			// Note: the instantiated item will have the string "(Clone)" attached to its name
			newItem = (Transform)Instantiate(
				(Transform)Resources.Load("Models/"+itemToLoad, typeof(Transform)));
		}
		equipmentList.Add(itemToLoad, targetLocation);
		// Get the weapon slot and parent the weapon to it and set its position and rotation to match
		Transform weaponSlot = photonView.transform.Find(targetLocation).transform;
		newItem.transform.parent = weaponSlot;
		newItem.transform.position = weaponSlot.position;
		newItem.transform.rotation = weaponSlot.rotation;		
	}
	
	private void GetTargetInfo(){
		
		Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit targetInfo;
		if (Physics.Raycast(mouseRay, out targetInfo)){
			PlayerObject info = targetInfo.transform.GetComponent<PlayerObject>();
			if (info != null){
				string curHealth = string.Format("Health: {0}", info.Health);
				GUI.Box(new Rect(Screen.width/2-50, 30, 100, 20), curHealth);
				GUI.Box(new Rect(Screen.width/2-info.Health/2, 60, info.Health, 20), "");
			} else if(targetInfo.transform.gameObject.tag == "Structure") {
				TowerObject tower = targetInfo.transform.gameObject.GetComponent<TowerObject>();
				string curHealth = string.Format("Health: {0}", tower.Health);
				GUI.Box(new Rect(Screen.width/2-50, 30, 100, 20), curHealth);
				GUI.Box(new Rect(Screen.width/2-tower.Health/2, 60, tower.Health, 20), "");
			}
		}
	}
	
	void OnGUI(){
		string curHealth = string.Format("Health: {0}/{1}", this.Health, this.maxHealth);
		GUI.Box(new Rect(Screen.width/2-50, Screen.height-50, 100, 20), curHealth);
		GUI.Box(new Rect(Screen.width/2-this.Health/2, Screen.height-30, this.Health, 20), "");
		GetTargetInfo();
	}
}
