using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepAI : MobileObject {

	Vector3 destination;
	Vector3 previousDestination;
	int waypointNumber = 1;
	int radius = 100;
	bool tracking = false;
	public List<DestructableObject> targetList = new List<DestructableObject>();
	public DestructableObject target;
	CreepVision vision;
	
	float logicUpdateTimer = 0f;
	public float logicUpdate;

	// Use this for initialization
	void Start () {

		this.maxHealth = 100;
		this.Health = this.maxHealth;

		if (PhotonNetwork.isMasterClient){
			SetNewDestination();
			RPCSendInitial();
			vision = transform.Find("VisionCollider").GetComponent<CreepVision>();
			vision.Setup();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		// Check if creep is in pursuit of a valid target
		if (tracking && target != null){
			if (logicUpdateTimer < logicUpdate){
				logicUpdateTimer += Time.deltaTime;
			} else {
				//TODO: Maintain some distance
					//TODO: Use cover and attempt to flank
				SetNewDestination(target.transform.position);
				logicUpdateTimer = 0f;
			}
		} else {
			// Set tracking bool if there's a target to track.
			tracking = FindNewTarget();
		}
	}
	
	bool FindNewTarget(){
		
		if (targetList.Count > 0){
			//TODO: Use real logic here.
			// Targets the first object it sees
			target = targetList[0];
			targetList.RemoveAt(0);
			return true;
		}
		
		return false;
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
	
	protected override void EndObject (){
		
		PhotonNetwork.Destroy(GetComponent<PhotonView>());
	}
}
