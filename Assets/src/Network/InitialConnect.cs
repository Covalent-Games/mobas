using UnityEngine;
using System.Collections;

public class InitialConnect : MonoBehaviour {

	static bool connected = false;
	
	public string IPAddress;
	public int port;
	public string AppID;
	public string version;

	// Use this for initialization
	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Informational;
		PhotonNetwork.ConnectToMaster(IPAddress, port, AppID, version);
	}
	
	void OnJoinedLobby(){
		if (PhotonNetwork.insideLobby){
			/* Use this statement to start the realtime map processes.
			* If this method gets called then the player is logged
			* in and ready to start querying the server for info */
			connected = true;
			Debug.Log("Connected to lobby");
		}
	}	
}