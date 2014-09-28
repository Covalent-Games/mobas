using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerHandler : MonoBehaviour {
	
	public static Dictionary<int, List<GameObject>> inventories = new Dictionary<int, List<GameObject>>();
	public static Dictionary<int, Dictionary<int, int>> attributes = new Dictionary<int, Dictionary<int, int>>();
	
	public float[] playerColor;

	
	void Start(){
		
		print("Enabled!");
		NetworkManager networkManager = 
		    GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
		if (!networkManager.isMaster){
			networkManager.TryJoinRoom();
		}
	}

	public static void SpawnPlayer(object content, int senderID){
		
		Vector3 spawnPoint = GameObject.Find("SpawnPoint").transform.position;
		GameObject player = (GameObject)PhotonNetwork.Instantiate (
			"CharacterObject",
			spawnPoint, 
			Quaternion.identity,
			0);
		PlayerObject playerObject = player.GetComponent<PlayerObject>();
		var info = (Dictionary<int, object>)content;
		
		/*string CharacterName = (string)info[GameEventParameter.CharacterName];
		playerObject.Actions = (IActions)player.gameObject.AddComponent(CharacterName);
		playerObject.Actions.RateOfFire = 8f;*/
		player.GetPhotonView().RPC (RPCName.PlayerSetup, PhotonTargets.All, player.GetPhotonView().viewID, senderID);
		EnableLocalControl(player);
		RegisterPlayerValues(player);
	}
	
	public static void EnableLocalControl(GameObject player){

		// Enable local scripts
		// Toggle MouseLook on (Use this to toggle during gameplay)
		//TODO Character needs to be loaded dynamically.. ish
		
		PlayerObject playerObject = player.GetComponent<PlayerObject>();
		
		playerObject.enabled = true;
		playerObject.mouseLookEnabled = true;
		playerObject.movementEnabled = true;
		playerObject.primaryActionEnabled = true;
		playerObject.actionsEnabled = true;
	}
	
	public static void RegisterPlayerValues(GameObject player){
	
		int viewId = PhotonView.Get(player).viewID;
		// Add new inventory list to the master inventory lists.
		PlayerHandler.inventories.Add(viewId, new List<GameObject>());
		
	}
	
	public static void SetCamera(GameObject player){
		
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
