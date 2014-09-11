using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerObject : StructureObject {

	float counter = 0f;
	float shotInterval = 2.0f;
	int damage = 8;
	List<GameObject> targeted = new List<GameObject>();


	// Use this for initialization
	void Start () {
		this.maxHealth = 200;
		this.Health = 200;
		SetRadius (10);
		base.Start ();
	}

	void Shoot(int damage, GameObject target) {
		if(PhotonNetwork.isMasterClient) {
			//Had to use target's photonview because it complained that it couldn't find DealDamage()
			target.GetComponent<PhotonView> ().RPC ("DealDamage", PhotonTargets.MasterClient, damage, target.GetComponent<PhotonView> ().viewID);
		}
	}

	void OnTriggerEnter(Collider collider) {

		targeted.Add (collider.gameObject);
		Debug.Log ("--Added " + collider.gameObject.name);
	}

	void OnTriggerStay(Collider collider) {

		if(counter >= shotInterval) {
			GameObject getsShot = collider.gameObject;
			float distance = Mathf.Infinity;
			Vector3 position = transform.position;
			Debug.Log("----List length: " + targeted.Count);
			foreach(var target in targeted) {
				//HACK: come up with an elegant way of removing targets outside foreach loop
				if(target != null) {
					Debug.Log("++++" + target.name + ": " + target.transform.position);
					Vector3 separation = target.transform.position - position;
					float currentDistance = separation.sqrMagnitude;
					if(currentDistance < distance) {
						getsShot = target;
						distance = currentDistance;
					}
				}
			}
			Vector3 towerVector = transform.position;
			Vector3 playerVector = getsShot.transform.position;
			Debug.Log("-----Shoots?");
			Debug.DrawLine (towerVector, playerVector, Color.red, 0.25f);
			Shoot (damage, getsShot);
			counter = 0f;
		}
	}

	void OnTriggerExit(Collider collider) {

		targeted.Remove (collider.gameObject);
		Debug.Log ("--Removed " + collider.gameObject.name);
	}


	void UpdateShotCounter() {

		counter += Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		UpdateShotCounter ();
	}
}
