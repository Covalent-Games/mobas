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
				
				break;
		}
	}
	
	void HandlePrimaryAction(object content){
	
		var info = (Dictionary<int, object>)content;
		int senderViewID = (int)info[GameEventParameter.SenderViewID];
		int targetViewID = (int)info[GameEventParameter.TargetViewID];
	
		Debug.Log(string.Format("PrimaryAction Triggered: Shooter:{0}, Target:{1}, {2}", senderViewID, targetViewID));
	}
}
