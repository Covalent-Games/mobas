using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepWaypoint : MonoBehaviour {

	public string colliderTag;
	public int waypointNumber;

	void OnTriggerEnter(Collider collider){
		
		//TODO Instead of checking for master client, the collider should only be present on the server
		if (PhotonNetwork.isMasterClient){
			if (collider.tag == colliderTag){
				collider.GetComponent<CreepAI>().SetNewDestination();
			}
		}
	}
}
