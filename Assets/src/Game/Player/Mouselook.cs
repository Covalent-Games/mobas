using UnityEngine;
using System.Collections;


/// <summary>
/// Controls mouse behaviour via Camera rotation.
/// </summary>
public class Mouselook : MonoBehaviour {

	float lookSensitivity = 5.0f;
	Transform parent;
	
	/// <summary>
	/// Sets the parent member. Call this AFTER MainCamera has been parented to Player.
	/// </summary>
	public void SetParentMember(){
		
		parent = transform.parent;
		lookSensitivity = parent.GetComponent<PlayerObject>().lookSensitivity;
	}

	protected void  RotatePlayer() {

		if (parent != null){
			if (parent.GetComponent<PlayerObject>().mouseLookEnabled){
				float mouseY = Input.GetAxis("Mouse Y") * -lookSensitivity;
				float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
				Transform player = transform.parent;
				transform.RotateAround(player.localPosition, player.right, mouseY);
				player.Rotate(new Vector3(0.0f, mouseX, 0.0f));
			}
		} else {
			SetParentMember();
		}
	}
	
	void Update(){
	
		RotatePlayer();
	}
}
