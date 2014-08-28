using UnityEngine;
using System.Collections;

public class MobileObject : DestructableObject {

	[SerializeField]
	protected CharacterController controller;
	
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
	protected void MoveObject(Vector3 direction){
		
		// This check might not be needed -- playing it safe for now
		if (direction != null){
			Vector3 newDirection = transform.TransformDirection(direction);
			controller.Move(newDirection);
		} else {
			Debug.LogError("Variable: 'direction' is null. How dare it be null!");
		}
	}
}
