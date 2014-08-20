using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {


	GameObject player;
	Vector3 spawnPoint;
	RoomOptions newRoomDetails;
	Vector3 playerColor;
	PhotonView photonView;
	
	void Start () {
		
		setRoomOptions();
		JoinRoom();
	}
	void OnJoinedRoom(){

		Debug.Log("Player joined room");
		playerColor = new Vector3(
				Random.Range(0.0f, 1.0f),
				Random.Range(0.0f, 1.0f),
				Random.Range(0.0f, 1.0f)); //color objects not serializable
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
		print(spawnPoint);
		player = (GameObject)PhotonNetwork.Instantiate (
			"Player",
			spawnPoint, 
			Quaternion.identity,
			0);
	}

	public void SetCamera(){
	
		Camera camera = GameObject.Find("MainCamera").camera;
		camera.GetComponent<Mouselook>().enabled = true;
		camera.transform.parent = player.transform;
		// Set the player's camera as the Main Camera
		camera.tag = "MainCamera";
		Vector3 pos = new Vector3(1.0f, 1.2f, -3.0f);
		camera.transform.position = player.transform.position + pos;
		camera.transform.rotation = new Quaternion(0, 0, 0 ,0);
	}

	void EnableLocalControl(){

		photonView = player.GetComponent<PhotonView>();
		if (photonView.isMine){
			// Enable local scripts
			player.GetComponent<AvatarMovement>().enabled = true;
			player.GetComponent<DemoShooting>().enabled = true;
			
			//This is bad. eventually we'll want to pass what/where to load based on character
			string locationToPlaceGun = "BackItem";
			string gunToLoad = "Gun_02";

			SetCamera();
			photonView.RPC("SetColor", PhotonTargets.AllBuffered, playerColor);
			photonView.RPC("SetItemLocation", PhotonTargets.AllBuffered, gunToLoad, locationToPlaceGun);

			player.gameObject.tag = "Player";
		}
	}
}
