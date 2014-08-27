using UnityEngine;
using System.Collections;

public class MasterRpcList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	[RPC]
	public void DealDamageToStructure(int damage, int targetID, PhotonMessageInfo info) {
		Debug.Log ("****Shooting tower: " + targetID.ToString());
		GameObject tower = GameObject.Find (targetID.ToString ());
		if(tower == null) {
			Debug.Log("****no such tower");
		} else {
			TowerShooting towerScript = tower.GetComponent<TowerShooting> ();
			towerScript.TakeDamage (damage);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
