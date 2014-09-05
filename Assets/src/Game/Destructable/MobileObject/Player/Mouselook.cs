﻿using UnityEngine;
using System.Collections;


/// <summary>
/// Controls mouse behaviour via Camera rotation.
/// </summary>
public class Mouselook : MonoBehaviour {

	float lookSensitivity = 5.0f;

	protected void  RotatePlayer() {

		float mouseY = Input.GetAxis("Mouse Y") * -lookSensitivity;
		float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
		Transform player = transform.parent;
		transform.RotateAround(player.localPosition, player.right, mouseY);
		player.Rotate(new Vector3(0.0f, mouseX, 0.0f));

	}
	
	void Update(){
	
		RotatePlayer();
	}
}
