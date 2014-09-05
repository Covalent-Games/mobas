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
		
		this.maxHealth = this.Health = 500;
			
		name = gameObject.GetInstanceID().ToString();
		if(PhotonNetwork.isMasterClient) {
			gameObject.AddComponent("MasterRpcList");
		}
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
		Dictionary<string, string> currentEquipment = photonView.GetComponent<AvatarAttributes>().equipmentList;
		Transform newItem;
		
		// If the location(value) is already occupied, remove the item(key) in it. 
		// TODO make this able to move the item to an unoccupied slot if possible instead of destroying it
		if (currentEquipment.ContainsValue(targetLocation)){
			foreach (string item in currentEquipment.Keys){
				if (currentEquipment[item] == targetLocation){
					Destroy(photonView.transform.FindChild(targetLocation).FindChild(
						item+"(Clone)").gameObject);
					currentEquipment.Remove(item);
					break;
				}
			}
		}
		// If we already have this item, reference the existing one instead of making another
		if (currentEquipment.ContainsKey(itemToLoad)){
			newItem = photonView.transform.FindChild(currentEquipment[itemToLoad]).FindChild(itemToLoad+"(Clone)");
			// Since we're moving the item we'll remove the entry in the avatars equipment list
			currentEquipment.Remove(itemToLoad);
			// The weapon doesn't already exist so we create it
		}else{
			// Note: the instantiated item will have the string "(Clone)" attached to its name
			newItem = (Transform)Instantiate(
				(Transform)Resources.Load("Models/"+itemToLoad, typeof(Transform)));
		}
		currentEquipment.Add(itemToLoad, targetLocation);
		// Get the weapon slot and parent the weapon to it and set its position and rotation to match
		Transform weaponSlot = photonView.transform.Find(targetLocation).transform;
		newItem.transform.parent = weaponSlot;
		newItem.transform.position = weaponSlot.position;
		newItem.transform.rotation = weaponSlot.rotation;		
	}
}
