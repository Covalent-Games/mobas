using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour {
	
	public static Dictionary<int, List<GameObject>> inventories = new Dictionary<int, List<GameObject>>();
	public static Dictionary<int, Dictionary<int, int>> attributes = new Dictionary<int, Dictionary<int, int>>();
	
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
		player.GetComponent<PlayerObject>().RPCSendInitial();
		GameObject.FindObjectOfType<GUIHandler>().enabled = true;
	}
	
	public void PlayerSetup(){
	
		EnableLocalControl();
		EnableLocalComponents();
		RegisterPlayerValues();
		SetCamera();
	}

	void EnableLocalControl(){

		PhotonView photonView = player.GetComponent<PhotonView>();
		if (photonView.isMine){
			// Enable local scripts
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
	
	void EnableLocalComponents(){
	
		//I made this and now I don't remember why. I think this method will have a purpose. Maybe.
	}
	
	void RegisterPlayerValues(){
	
		int viewId = PhotonView.Get(player).viewID;
		// Add new inventory list to the master inventory lists.
		PlayerHandler.inventories.Add(viewId, new List<GameObject>());
		
	}
	
	void SetCamera(){
		
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
	
	public void Update(){}
}
