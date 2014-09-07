using UnityEngine;
using System.Collections;

public class MasterRpcList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	[RPC]
	public void DealDamageToStructure(int damage, int viewID, PhotonMessageInfo info) {
		Debug.Log ("****Shooting tower: " + viewID);
		GameObject tower = PhotonView.Find (viewID).gameObject;
		if(tower == null) {
			Debug.Log("****no such tower");
		} else {
			TowerObject towerObject = tower.GetComponent<TowerObject> ();
			towerObject.Health -= damage;
			Debug.Log("****hit tower: " + tower.name);
		}
		
	}

	[RPC]
	public void DestroySceneObject(PhotonMessageInfo info) {
		print ("---MasterRPCList DSO");
		string methodName;
		switch((DestructableObject.groupID)info.photonView.group) {
			case DestructableObject.groupID.towers: 
				methodName = "DestroyTower";
				print ("----methodName = " + methodName);
				break;
			default:
				methodName = "";
				print ("----methodName = " + methodName);
				break;
		}
		info.photonView.RPC (methodName, PhotonTargets.All, DestructableObject.groupID.towers);
	}
	/*
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
	*/

	// Update is called once per frame
	void Update () {
	
	}
}
