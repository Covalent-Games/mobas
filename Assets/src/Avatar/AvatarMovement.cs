using UnityEngine;
using System.Collections;

public class AvatarMovement : MonoBehaviour {

	CharacterController controller;
	[SerializeField]
	float movementSpeed = 15.0f;
	[SerializeField]
	float lookSensitivity = 5.0f;
	[SerializeField]
	float diagonalResponsiveness = 0.09f;
	[SerializeField]
	float stickynessToGround = 0.09f;
	[SerializeField]
	float inertia = 0.981f;
	
	float gravity;
	float lookx;
	float momentumX = 0.0f;
	float momentumY = 0.0f;
	
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	void move(){
		float time = Time.deltaTime;
		float moveX = momentumX;
		float moveY = momentumY;
		if (controller.isGrounded) {
			gravity = -stickynessToGround;
		
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
		
		Vector3 direction = new Vector3 (moveX, gravity, moveY);
		controller.Move(transform.TransformDirection(direction));
	}

	void Update () {
		move();
	}
}
