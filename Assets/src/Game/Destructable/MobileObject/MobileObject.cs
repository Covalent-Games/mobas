using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobileObject : DestructableObject {

	[SerializeField]
	protected CharacterController controller;
	public bool movementEnabled = false;
	public bool mouseLookEnabled = false;
	public bool primaryActionEnabled = false;
	public bool actionsEnabled = false;
	
	public float globalCooldown = 1f;
	public float globalCooldownTimer = 0f;
	
	//TEST
	public string CharacterName;
	public IActions Actions;
	
	void Start () {
	
		controller = GetComponent<CharacterController>();
		AssignNewControllerIfNeeded(controller);
	}
	
	private void AssignNewControllerIfNeeded(CharacterController controller){
		
		if (controller == null){
			// This theoretically could get the appropriate controller specific to the object
			gameObject.AddComponent<CharacterController>();
			Debug.LogWarning(string.Format("No CharacterController assigned to {0} -- Default CharacterController added.", gameObject.name));
		}
	}
	
	/// <summary>
	/// Moves the object in a specified direction.
	/// </summary>
	/// <param name="direction">Direction to move player. Being a Vector3 this includes velocity.</param>
	public void MoveObject(Vector3 direction){
		
		Vector3 correctedDirection = transform.TransformDirection(direction);
		controller.Move(correctedDirection);
	}
	
	[RPC]
	public void NetworkMoveObject(Vector3 newLocation, PhotonMessageInfo info){
	
		if (info.photonView.viewID == PhotonView.Get(this).viewID){
			this.transform.position = newLocation;
		}
	}
	
	[RPC]
	public void NetworkRotateObject(Quaternion newRotation, PhotonMessageInfo info){
		
		if (info.photonView.viewID == PhotonView.Get(this).viewID){
			this.transform.rotation = newRotation;
		}
	}
}
