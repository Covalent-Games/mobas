using UnityEngine;
using System.Collections;

public class GameEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		if (PhotonNetwork.isMasterClient){
			Debug.Log("OnEventRaised registered.");
			PhotonNetwork.OnEventCall += this.OnEventRaised;
		}
		
	}
	
	public void OnEventRaised(byte eventCode, object content, int senderID){
	
		switch ((GameEventCode)eventCode){
			default:
				Debug.LogError("Unkown eventCode");
				break;
			case GameEventCode.TestEvent:
				Debug.Log(string.Format("OnEventRaised: {0}, {1}, {2}", eventCode, content, senderID));
				break;
		}
	}

}
