using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

//TODO: Player and Master connection logic HAVE to be separated... And the mystery of how anything works needs to be solved.
public class NetworkManager : MonoBehaviour {

	//TODO Server spawns first instance
	public bool isMaster;
	public bool developer;
	public string IPAddress;
	public int port;
	public string AppID;
	public string version;
	bool masterClientInitiated = false;

	void Awake(){

		DontDestroyOnLoad(this);
	}
	
	void Start(){
	
		if (this.isMaster | this.developer){
			InitiateConnection();
		}
	}
	
	/// <summary>
	/// Initiates the connection.
	/// </summary>
	public void InitiateConnection () {

		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectToMaster(IPAddress, port, AppID, version);
	}
	
	/// <summary>
	/// Called on completion of the joined lobby event.
	/// </summary>
	void OnJoinedLobby(){
		
		if (PhotonNetwork.insideLobby){
			print ("Joined the lobby... AAHHHH!!");
			if (!isMaster){
				User.currentState = User.State.ConnectedToMaster;
				/* Use this statement to start the realtime map processes.
				* If this method gets called then the player is logged
				* in and ready to start querying the server for info */
				if (PhotonNetwork.logLevel == PhotonLogLevel.Full){
					Debug.Log("Connected to lobby");
				}
				// Currently the only way to execute the request. 
				//PhotonNetwork.networkingPeer.OpCustom((byte)LogicOperationCode.GetSectorInfo, new Dictionary<byte, object>(), true);
				
				// If Developer box is checked in the inspector, skip straight to the arena level.
				if (developer){
					Application.LoadLevel("main");
				}
			} else {
				//TODO: This will obviously need to know correct level to load at runtime.
				//HACK: Don't hardode room name
				PhotonNetwork.CreateRoom("room0");
				Debug.Log("Create the rooooooommmmm!");
			}
		}
	}
	
	public void TryJoinRoom(){
		
		//TODO: Get name of room ... not like this. This is bad.
		Debug.Log ("Joining room...");
		PhotonNetwork.JoinRoom("room0");
	}
	
	void OnJoinedRoom(){
		
		if (isMaster){
			if (PhotonNetwork.logLevel == PhotonLogLevel.Full){
				Debug.Log("Master joined room");
			}
			return;
		}
		
		if (PhotonNetwork.logLevel == PhotonLogLevel.Full){
			Debug.Log("Player joined room");
		}
		
		PlayerHandler playerHander = GameObject.Find("PlayerhandlerObject").GetComponent<PlayerHandler>();
		
		playerHander.playerColor = new Vector3(
			Random.Range(0.0f, 1.0f),
			Random.Range(0.0f, 1.0f),
			Random.Range(0.0f, 1.0f)); //color objects not serializable
		playerHander.SpawnPlayer();
		playerHander.EnableLocalControl();
	}
	
	void OnLevelWasLoaded(){
	
		if (Application.loadedLevelName == "main"){
			GameObject.FindObjectOfType<SceneHandler>().Begin();
			RegisterEvents();
		}
	}
	
	void RegisterEvents (){
		
		PhotonNetwork.OnEventCall += GameObject.Find("MasterObject").GetComponent<GameEventHandler>().OnEventRaised;
	}
	
	void OnPhotonJoinRoomFailed(){
		
		if(!masterClientInitiated) {
			var parameter = new Dictionary<byte, object>();
			parameter.Add(LogicParameterCode.RoomID, "room0");
			PhotonNetwork.networkingPeer.OpCustom((byte)LogicOperationCode.SpawnMasterClientProcess, parameter, true);
			masterClientInitiated = true;
		}
		Debug.Log ("If at first you don't succeed...");
		//PhotonNetwork.JoinRoom("room0");
	}
	
	void OnCreatedRoom() {
		
		//print ("Joined room called");
		//PhotonNetwork.JoinRoom("room0");
		Application.LoadLevel("main");
	}
	
	/// <summary>
	/// Disconnects from server while resetting the User's name and tada!
	/// setting their state to Unidentified.
	/// </summary>
	public void DisconnectFromServer(){
		
		User.Username = "";
		User.currentState = User.State.Unidentified;
		// This should hard disconnect from everything. If not LeaveRoom() can be called.
		PhotonNetwork.networkingPeer.Disconnect();
	}
}
