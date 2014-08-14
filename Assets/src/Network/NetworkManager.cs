using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {
	
	public string IPAddress;
	public int port;
	public string AppID;
	public string version;

	void Awake(){
		DontDestroyOnLoad(this);
	}
	/// <summary>
	/// Initiates the connection.
	/// </summary>
	public void InitiateConnection () {

		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectToMaster(IPAddress, port, AppID, version);
	}
	
	/// <summary>
	/// Disconnects from server while resetting the User's name and
	/// setting their state to Unidentified.
	/// </summary>
	public void DisconnectFromServer(){
		
		User.Username = "";
		User.currentState = User.State.Unidentified;
		// This should hard disconnect from everything. If not LeaveRoom() can be called.
		PhotonNetwork.networkingPeer.Disconnect();
	}
	
	/// <summary>
	/// Called on completion of the joined lobby event.
	/// </summary>
	void OnJoinedLobby(){

		if (PhotonNetwork.insideLobby){
			User.currentState = User.State.ConnectedToMaster;
			/* Use this statement to start the realtime map processes.
			* If this method gets called then the player is logged
			* in and ready to start querying the server for info */
			Debug.Log("Connected to lobby");
			// parameters contains ALL the data you want to send the server for this operation
			var parameters = new Dictionary<byte, object>();
			// TestRequest helps the server identify what to do with 'var parameters'
			parameters.Add(LogicParameterCode.TestRequest, (byte)0);
			Debug.Log(string.Format("Sending Operation {0}", OperationCode.GameLogicOperation));
			// Currently the only way to execute the request. 
			PhotonNetwork.networkingPeer.OpCustom(OperationCode.GameLogicOperation, parameters, true);
		}
	}
}