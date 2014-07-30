using UnityEngine;
using System.Collections;

public class CameraXPivot : MonoBehaviour {

	float lookSensitivity = -5.0f;

	void Update () {
		float mouseY = Input.GetAxis("Mouse Y");
		Vector3 rotation = transform.localEulerAngles;
		rotation.x += mouseY * lookSensitivity;
		transform.localEulerAngles = rotation;
	}
}
