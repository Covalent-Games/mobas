using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {


	GameObject player;
	Vector3 spawnPoint;
	RoomOptions newRoomDetails;
	Vector3 playerColor;
	PhotonView photonView;
	
	void Start () {
		
		//TODO Needs to be in network
		setRoomOptions();
		JoinRoom();
	}
	
	//TODO Needs to be in network
	void OnJoinedRoom(){

		if (PhotonNetwork.logLevel == PhotonLogLevel.Full){
			Debug.Log("Player joined room");
		}
		playerColor = new Vector3(
				Random.Range(0.0f, 1.0f),
				Random.Range(0.0f, 1.0f),
				Random.Range(0.0f, 1.0f)); //color objects not serializable
		SpawnPlayer();
		EnableLocalControl();

		//HACK THis is STRICTLY for testing
		if (PhotonNetwork.isMasterClient){
			PhotonNetwork.OnEventCall += GameObject.Find("MasterObject").GetComponent<GameEventHandler>().OnEventRaised;
		}
	}
	
	//TODO Needs to be in network
	void setRoomOptions(){

		newRoomDetails = new RoomOptions ();
	}
	
	//TODO Needs to be in network
	void JoinRoom(){

		PhotonNetwork.JoinOrCreateRoom("Yeeha!",
		                               newRoomDetails,
		                               TypedLobby.Default);
	}	
	void SpawnPlayer(){

		spawnPoint = GameObject.Find("SpawnPoint").transform.position;
		player = (GameObject)PhotonNetwork.Instantiate (
			"CharacterObject",
			spawnPoint, 
			Quaternion.identity,
			0);
		SceneHandler sceneHandler = GameObject.FindObjectOfType<SceneHandler>();
		sceneHandler.Begin ();
	}

	public void SetCamera(){
	
		Camera camera = GameObject.Find("MainCamera").camera;
		camera.tag = "MainCamera";
		camera.transform.parent = player.transform;
		// Turn on the MouseLook component for this client (Don't change this)
		camera.GetComponent<Mouselook>().enabled = true;
		// Toggle MouseLook on (Use this to toggle during gameplay)
		//TODO Character needs to be loaded dynamically.. ish
		PlayerObject playerObject = player.GetComponent<PlayerObject>();
		playerObject.mouseLookEnabled = true;
		playerObject.movementEnabled = true;
		playerObject.primaryActionEnabled = true;
		playerObject.actionsEnabled = true;
		
		// Set the player's camera as the Main Camera
		Vector3 pos = new Vector3(1.0f, 1.2f, -3.0f);
		camera.transform.position = player.transform.position + pos;
		camera.transform.rotation = new Quaternion(0, 0, 0 ,0);
	}

	void EnableLocalControl(){

		photonView = player.GetComponent<PhotonView>();
		if (photonView.isMine){
			// Enable local scripts
			//TODO Eventually we'll want to pass what/where to load based on character
			string locationToPlaceGun = "ReadiedItem";
			string gunToLoad = "demoGun_01";

			SetCamera();
			photonView.RPC("SetColor", PhotonTargets.AllBuffered, playerColor);
			photonView.RPC("SetItemLocation", PhotonTargets.AllBuffered, gunToLoad, locationToPlaceGun);

			player.gameObject.tag = "Player";
		}
	}
}
