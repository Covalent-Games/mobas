using UnityEngine;
using System.Collections;

public class AvatarMovement : MonoBehaviour {

	CharacterController controller;
	[SerializeField]
	float movementSpeed = 10.0f;
	[SerializeField]
	float lookSensitivity = 5.0f;
	[SerializeField]
	float diagonalResponsiveness = 0.09f;
	float gravity;
	float lookx;

	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	void move(){
		float time = Time.deltaTime;
		if (controller.isGrounded) {
			gravity = 0.0f;
		} else {
			gravity -= 9.81f * time * time;
		}

		float moveX = Input.GetAxis("Horizontal") * movementSpeed * time;
		float moveY = Input.GetAxis("Vertical") * movementSpeed * time;

		// Diagonal movement compensation
		if (Mathf.Abs(moveX) > diagonalResponsiveness && Mathf.Abs(moveY) > diagonalResponsiveness){
			float diagonal_move = Mathf.Sqrt(moveX * moveX + moveY * moveY);
			moveX = diagonal_move / 2 * Mathf.Sign(moveX);
			moveY = diagonal_move / 2 * Mathf.Sign(moveY);
		}

		Vector3 direction = new Vector3 (moveX, gravity, moveY);

		controller.Move(transform.TransformDirection(direction));
	}

	void Update () {
		move();
	}
}
