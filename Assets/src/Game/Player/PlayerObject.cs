using UnityEngine;
using System.Collections;

public class PlayerObject : MobileObject {

	[SerializeField]
	PhotonView photonView;
	[SerializeField]
	float movementSpeed = 10.0f;
	[SerializeField]
	float lookSensitivity = 5.0f;
	[SerializeField]
	float diagonalResponsiveness = 0.09f;
	[SerializeField]
	float inertia = 0.981f;
	
	float gravity;
	float lookx;
	float momentumX = 0.0f;
	float momentumY = 0.0f;
	
	public void Start(){

	}
	
	void Move(){
	
		float time = Time.deltaTime;
		float moveX = momentumX;
		float moveY = momentumY;
		if (this.controller.isGrounded) {			
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
		
		MoveObject(new Vector3(momentumX, 0, momentumY));
	}
	
	public void Update(){
	
		Move();
	}
}
