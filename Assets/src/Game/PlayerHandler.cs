using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour {


	GameObject player;
	Vector3 spawnPoint;
	public float[] playerColor;

	
	void Start(){
		
		print("Enabled!");
		NetworkManager networkManager = 
		    GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
		if (!networkManager.isMaster){
			networkManager.TryJoinRoom();
		}
	}

	public void SpawnPlayer(){

		spawnPoint = GameObject.Find("SpawnPoint").transform.position;
		player = (GameObject)PhotonNetwork.Instantiate (
			"CharacterObject",
			spawnPoint, 
			Quaternion.identity,
			0);
		GameObject.FindObjectOfType<GUIHandler>().enabled = true;
	}

	public void SetCamera(){
	
		Camera camera = GameObject.Find("MainCamera").camera;
		camera.tag = "MainCamera";
		camera.transform.parent = player.transform;
		// Turn on the MouseLook component for this client (Don't change this)
		camera.GetComponent<Mouselook>().enabled = true;
		// Set the player's camera as the Main Camera
		Vector3 pos = new Vector3(1.0f, 1.2f, -3.0f);
		camera.transform.position = player.transform.position + pos;
		camera.transform.rotation = new Quaternion(0, 0, 0 ,0);
	}

	public void EnableLocalControl(){

		PhotonView photonView = player.GetComponent<PhotonView>();
		if (photonView.isMine){
			// Enable local scripts
			SetCamera();
			// Toggle MouseLook on (Use this to toggle during gameplay)
			//TODO Character needs to be loaded dynamically.. ish
			
			player.gameObject.tag = "Player";
			
			PlayerObject playerObject = player.GetComponent<PlayerObject>();
			playerObject.enabled = true;
			playerObject.mouseLookEnabled = true;
			playerObject.movementEnabled = true;
			playerObject.primaryActionEnabled = true;
			playerObject.actionsEnabled = true;
		}
	}
	
	public void Update(){}
}
