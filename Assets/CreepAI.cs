using UnityEngine;
using System.Collections;

public class CreepAI : MobileObject {

	Vector3 destination;
	int radius = 100;

	// Use this for initialization
	void Start () {

		SetNewDestination();
		maxHealth = 100;
		Health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void SetNewDestination(){
		
		destination = Vector3.zero;
		Vector2 newPos = Random.insideUnitCircle * radius;
		destination.x += newPos.x + 250;
		destination.z += newPos.y + 250;
		GetComponent<NavMeshAgent>().SetDestination(destination);
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
			//HACK -- This needs to be handled by master
			PhotonNetwork.Destroy(GetComponent<PhotonView>());
		}
	}
}
