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

	[RPC]
	public void DealDamage(int damageDealt, int ID){
		
		if (ID == GetComponent<PhotonView>().owner.ID){
			
			GetComponent<MobileObject>().Health -= damageDealt;
		}
	}
	
	/// <summary>
	/// Moves the object in a specified direction.
	/// </summary>
	/// <param name="direction">Direction to move player. Being a Vector3 this includes velocity.</param>
	protected void MoveObject(Vector3 direction){
		
		Vector3 newDirection = transform.TransformDirection(direction);
		controller.Move(newDirection);
	}
}
