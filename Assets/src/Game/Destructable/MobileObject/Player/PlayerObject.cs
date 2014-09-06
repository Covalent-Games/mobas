using UnityEngine;
using System.Collections;

public class PlayerObject : MobileObject {


	public float movementSpeed = 10.0f;
	public float lookSensitivity = 5.0f;
	[SerializeField]
	float diagonalResponsiveness = 0.05f;
	[SerializeField]
	float inertia = 0.981f;
	
	float gravity;
	float lookx;
	float momentumX = 0.0f;
	float momentumY = 0.0f;
	public bool mouseLookEnabled = false;
	
	public void Start(){
		
		this.maxHealth = this.Health = 500;
		name = gameObject.GetInstanceID().ToString();
		if(PhotonNetwork.isMasterClient) {
			gameObject.AddComponent<MasterRpcList>();
		}
	}
	
	void Move(){
	
		float time = Time.deltaTime;
		float moveX = momentumX;
		float moveY = momentumY;
		if (this.controller.isGrounded && movementEnabled) {
			gravity = 0f;		
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
		
		MoveObject(new Vector3(momentumX, gravity, momentumY));
	}
	
	public void Update(){

		Move();
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
