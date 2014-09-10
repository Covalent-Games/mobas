using UnityEngine;
using System.Collections;

public class MasterRpcList : MonoBehaviour {

	[RPC]
	public void DealDamageToStructure(int damage, int targetID, PhotonMessageInfo info) {
		Debug.Log ("****Shooting tower: " + targetID.ToString());
		GameObject tower = GameObject.Find (targetID.ToString ());
		if(tower == null) {
			Debug.Log("****no such tower");
		} else {
			TowerObject towerScript = tower.GetComponent<TowerObject> ();
			towerScript.Health -= damage;
		}
	}
	
	[RPC]
	public void DealDamageToMobile(int damage, PhotonMessageInfo info){
	
		//Probably more security logic needed here...
		info.photonView.RPC("DealDamage", PhotonTargets.All, damage);
		
		print("Master will protect us, precioussss");
	}

}
