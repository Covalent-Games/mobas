using UnityEngine;
using System.Collections;

public class CreepVision : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		
		DestructableObject target = collider.GetComponent<DestructableObject>();
		DestructableObject parent = transform.parent.GetComponent<DestructableObject>();
		
		if (target.faction != parent.faction){
			Debug.Log("PEWPEWPEWPEPW");	
		}
	}
}
