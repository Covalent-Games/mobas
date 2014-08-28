using UnityEngine;
using System.Collections;


/// <summary>
/// Controls mouse behaviour via Camera rotation.
/// </summary>
public class Mouselook : MonoBehaviour {

	float lookSensitivity = 5.0f;
	Transform player;

	public Mouselook(Transform player){
		
		if (player == null){
			Debug.LogError("Player not set");
		} else {
			this.player = player;
		}
	}

	protected void  RotatePlayer() {
		float mouseY = Input.GetAxis("Mouse Y") * -lookSensitivity;
		float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
		Transform player = transform.parent;
		transform.RotateAround(player.localPosition, player.right, mouseY);
		player.Rotate(new Vector3(0.0f, mouseX, 0.0f));
	}
}
