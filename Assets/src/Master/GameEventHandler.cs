using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
				Debug.LogError("! Unkown eventCode !");
				break;
			case GameEventCode.PrimaryAction:
				Debug.Log(string.Format("OnEventRaised: {0}, {1}, {2}", eventCode, content, senderID));
				break;
		}
	}


}
