using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {


	GameObject player;
	Vector3 spawnPoint;
	RoomOptions newRoomDetails;
	Color playerColor;
	
	void Start () {
		
		setRoomOptions();
		JoinRoom();
		
	}
	void OnJoinedRoom(){

		Debug.Log("Player joined room");
		playerColor = new Color(
				Random.Range(0.0f, 1.0f),
				Random.Range(0.0f, 1.0f),
				Random.Range(0.0f, 1.0f));
		SpawnPlayer();
		EnableLocalControl();
	}
	void setRoomOptions(){

				newRoomDetails = new RoomOptions ();
	}
	
	void JoinRoom(){

		PhotonNetwork.JoinOrCreateRoom("Yeeha!",
		                               newRoomDetails,
		                               TypedLobby.Default);
	}	
	void SpawnPlayer(){

		spawnPoint = GameObject.Find("SpawnPoint").transform.position;
		player = (GameObject)PhotonNetwork.Instantiate (
			"Player",
			spawnPoint, 
			Quaternion.identity,
			0);
		player.transform.renderer.material.color = playerColor;
	}
	
	void EnableLocalControl(){

		PhotonView pv = player.GetComponent<PhotonView>();
		Transform cameraPivot = player.transform.Find("CameraPivot");

		if (pv.isMine){
			// Enable local scripts
			player.GetComponent<AvatarMovement>().enabled = true;
			cameraPivot.GetComponent<CameraXPivot>().enabled = true;
			player.GetComponent<DemoShooting>().enabled = true;

			Camera camera = GameObject.Find("MainCamera").camera;
			camera.transform.parent = cameraPivot;
			// Set the player's camera as the Main Camera
			camera.tag = "MainCamera";
			Vector3 pos = new Vector3(1.0f, 1.0f, -3.5f);
			camera.transform.position = cameraPivot.transform.position + pos;
			player.gameObject.tag = "Player";
		}
	}
}
