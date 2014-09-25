﻿using UnityEngine;
using System.Collections;

public class TowerVision : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
	
		//HACK: This is just nasty
		DestructableObject target = collider.GetComponent<DestructableObject>();
		if (target != null){
			if (target.faction == this.GetComponentInParent<DestructableObject>().faction){ return; }
		}

		transform.parent.GetComponent<TowerObject>().targeted.Add (collider.gameObject);
		//Debug.Log ("--Added " + collider.gameObject.name);
	}
	
	void OnTriggerExit(Collider collider) {
		
		transform.parent.GetComponent<TowerObject>().targeted.Remove (collider.gameObject);
		//Debug.Log ("--Removed " + collider.gameObject.name);
	}
}