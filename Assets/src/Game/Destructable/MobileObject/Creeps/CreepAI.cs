using UnityEngine;
using System.Collections;

public class CreepAI : MobileObject {

	Vector3 destination;
	Vector3 previousDestination;
	int waypointNumber = 1;
	int radius = 100;

	// Use this for initialization
	void Start () {

		this.maxHealth = 100;
		this.Health = this.maxHealth;

		if (PhotonNetwork.isMasterClient){
			SetNewDestination();
			Debug.Log("SendingInitial");
			RPCSendInitial();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		//AI logic in here -- Make sure this script is disabled by default
	}
	
	/// <summary>
	/// Sets the new destination.
	/// </summary>
	public void SetNewDestination(){
		
		GameObject[] waypoints = GameObject.FindGameObjectsWithTag("CreepWaypoint");

		foreach (GameObject waypoint in waypoints){
			// If waypointNumbers match, this is the next waypoint to move toward
			//TODO This needs to include logic for teams. It might be something as simple
			// as tagging the waypoints for each team instead of explicitly querying "CreepWaypoints".
			if (waypoint.GetComponent<CreepWaypoint>().waypointNumber == waypointNumber){
				GetComponent<NavMeshAgent>().SetDestination(waypoint.transform.position);
				if (waypointNumber == waypoints.Length){
					// There are no more waypoints, so go back to the first one once
					// AI has reached its destination.
					waypointNumber = 1;
				} else {
					waypointNumber++;
				}
				// Remember where it came from
				previousDestination = transform.position;
			}
		}
	}
	
	/// <summary>
	/// Sets the new destination.
	/// </summary>
	/// <param name="destination">New Vector3 destination.</param>
	public void SetNewDestination(Vector3 destination){
		
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
		
		if (destination == null){
			navMeshAgent.SetDestination(previousDestination);
			Debug.LogWarning("Destination not provided... creep is going home");
		}
		
		navMeshAgent.SetDestination(destination);
		
		previousDestination = transform.position;
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
	
	protected override void CheckIfDestroyed (){
		
		if (Health <= 0){
			PhotonNetwork.Destroy(GetComponent<PhotonView>());
		}
	}
}
