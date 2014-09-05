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

	void shoot(int damage, GameObject target) {

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
			Debug.Log("--List length: " + targeted.Count);
			foreach(var target in targeted) {
				if(target == null) {
					targeted.Remove(target);
				} else {
					Debug.Log("--Check " + target.name);
					Vector3 separation = target.transform.position - position;
					float currentDistance = separation.sqrMagnitude;
					if(currentDistance < distance) {
						getsShot = target;
						currentDistance = distance;
					}
				}
			}
			Vector3 towerVector = transform.position;
			Vector3 playerVector = getsShot.transform.position;
			Debug.Log("-----Shoots?");
			Debug.DrawLine (towerVector, playerVector, Color.red, 0.25f);
			shoot (damage, getsShot);
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
