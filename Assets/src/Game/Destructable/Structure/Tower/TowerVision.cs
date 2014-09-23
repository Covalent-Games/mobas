using UnityEngine;
using System.Collections;

public class TowerVision : MonoBehaviour {

	TowerObject parent;

	void OnTriggerEnter(Collider collider) {
		
		transform.parent.GetComponent<TowerObject>().targeted.Add (collider.gameObject);
		//Debug.Log ("--Added " + collider.gameObject.name);
	}
	
	void OnTriggerExit(Collider collider) {
		
		transform.parent.GetComponent<TowerObject>().targeted.Remove (collider.gameObject);
		//Debug.Log ("--Removed " + collider.gameObject.name);
	}
}
