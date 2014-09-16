using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {
	
	//TODO Server spawns first instance
	public bool isMaster;
	public bool developer;
	public string IPAddress;
	public int port;
	public string AppID;
	public string version;

	void Awake(){
		DontDestroyOnLoad(this);
	}
	
	void Start(){
	
		if (this.developer | this.isMaster){
			InitiateConnection();
		}
	}
	
	/// <summary>
	/// Initiates the connection.
	/// </summary>
	public void InitiateConnection () {

		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
		PhotonNetwork.ConnectToMaster(IPAddress, port, AppID, version);
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
	
	void OnFailedToConnectToPhoton(){
		Debug.Log("We be failin...");
	}
	
	/// <summary>
	/// Called on completion of the joined lobby event.
	/// </summary>
	void OnJoinedLobby(){

		if (PhotonNetwork.insideLobby){
			if (!isMaster){
				User.currentState = User.State.ConnectedToMaster;
				/* Use this statement to start the realtime map processes.
				* If this method gets called then the player is logged
				* in and ready to start querying the server for info */
				if (PhotonNetwork.logLevel == PhotonLogLevel.Full){
					Debug.Log("Connected to lobby");
				}
				// Currently the only way to execute the request. 
				PhotonNetwork.networkingPeer.OpCustom((byte)LogicOperationCode.GetSectorInfo, new Dictionary<byte, object>(), true);
				
				// If Developer box is checked in the inspector, skip straight to the arena level.
				if (developer){
					Application.LoadLevel("main");
				}
			} else {
				//TODO: This will obviously need to know correct level to load at runtime.
				Application.LoadLevel("main");
			}
		}
	}
}