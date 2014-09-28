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
				HandlePrimaryAction(content);
				break;
			case GameEventCode.SpawnPlayer:
				HandleSpawnPlayer(content, senderID);
				break;
			case GameEventCode.MovePlayer:
				HandleMovePlayer(content);
				break;
		}
	}
	
	void HandlePrimaryAction(object content){
		
		var info = (Dictionary<int, object>)content;
		int senderViewID = (int)info[GameEventParameter.SenderViewID];
		int targetViewID = (int)info[GameEventParameter.TargetViewID];
		PhotonView targetPhotonView = PhotonView.Find(targetViewID);
		
		if (targetPhotonView == null){
			Debug.LogWarning("DestructableObject/PhotonView not found during HandlePrimaryAction");
			return;
		}

		DestructableObject shooter = PhotonView.Find(senderViewID).GetComponent<DestructableObject>();
		Debug.Log("----shooter: " + senderViewID);		
		Debug.Log("----shooter stats: " + shooter.Health + ", " + shooter.targetDamage + ", " +
					shooter.areaDamage + ", " + shooter.healing);

		int damage = shooter.targetDamage;
		//int damage = PhotonView.Find(senderViewID).GetComponent<DestructableObject>().targetDamage;
		DestructableObject target = targetPhotonView.GetComponent<DestructableObject>();
		
		Debug.Log("Damage before defence: " + damage);
		float finalDamage = damage * ((100-target.defence) / 100.0f);
		Debug.Log("Damage after defence: " + finalDamage);
		int newHealth = target.Health - Mathf.RoundToInt(finalDamage);
		

		//TODO: Incorporate character attributes in damage calculation
		Dictionary<int, object>parameters = new Dictionary<int, object>();
		parameters.Add(GameEventParameter.Health, newHealth);
		targetPhotonView.RPC ("UpdateInfo", PhotonTargets.All, parameters);
		
	}

	void HandleSpawnPlayer(object content, int senderID) {

		PlayerHandler.SpawnPlayer(content, senderID);
	}
	
	void HandleMovePlayer(object content){
	
		var info = (Dictionary<int, object>)content;
		int viewID = (int)info[GameEventParameter.SenderViewID];
		
		PhotonView photonView = PhotonView.Find(viewID);
		
		if (photonView == null) {
			Debug.LogError("PhotonView: " + viewID + " Not found -- This shouldn't be happening ever!");
		}
		
		PlayerObject player = photonView.gameObject.GetComponent<PlayerObject>();
		
		player.Move((float)info[GameEventParameter.Horizontal], (float)info[GameEventParameter.Vertical]);
	}
}


