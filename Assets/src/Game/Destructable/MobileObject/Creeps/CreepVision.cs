using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepVision : MonoBehaviour {

	CreepAI parent;
	public bool canSee = false;
	public List<DestructableObject> visible = new List<DestructableObject>();
	
	public void Setup(){
		
		parent = transform.parent.GetComponent<CreepAI>();
		enabled = true;
		canSee = true;
	}

	void OnTriggerEnter(Collider collider){
		
		if (!canSee){ return; }

		DestructableObject target = collider.GetComponent<DestructableObject>();
		
		// Check if the target is of opposing faction and not already in targetList.
		if (target.faction != parent.faction && !parent.targetList.Contains(target)){
			parent.targetList.Add(target);
			
			// clean targetList of null objects
			parent.targetList.RemoveAll(item => item == null);
		}
	}
	
	void OnTriggerExit(Collider collider){
	
		DestructableObject target = collider.GetComponent<DestructableObject>();
		
		//FIXME: This will not handle player disconnects
		if (parent.targetList.Contains(target)){
			parent.targetList.Remove(target);
			
			// clean targetList of null objects
			parent.targetList.RemoveAll(item => item == null);
		}
		
		if (target == parent.target){
			parent.target = null;
			parent.SetNewDestination();
		}
	}
}
