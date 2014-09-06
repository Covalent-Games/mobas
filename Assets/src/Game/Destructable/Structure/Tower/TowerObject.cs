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
		this.GetComponent<PhotonView> ().group = (int)groupID.towers;
		this.maxHealth = 200;
		this.Health = 200;
		SetRadius (10);
		base.Start ();

		//PhotonNetwork.AllocateViewID ();
		//PhotonNetwork.LoadLevel ();
		//PhotonNetwork.JoinRoom ("Yeeha!");
		//PhotonNetwork.InstantiateSceneObject ("Tower", transform.position, transform.rotation, 1, null);
		/*tower = (GameObject)PhotonNetwork.Instantiate (
			"Tower",
			transform.position, 
			Quaternion.identity,
			1);	
		*/
	}

	public void InitializeScene() {

	}

	void Shoot(int damage, GameObject target) {

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
			Shoot (damage, getsShot);
			counter = 0f;
		}
	}

	void OnTriggerExit(Collider collider) {

		targeted.Remove (collider.gameObject);
		Debug.Log ("--Removed " + collider.gameObject.name);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		//Debug.Log ("###Serializing view?");
		if (stream.isWriting){
			int h = Health;
			//Debug.Log("###Tower health: " + h);
			stream.SendNext (h);
		} else {
			//Debug.Log("###Receiving health");
			Health = (int)stream.ReceiveNext();
		}
	}
	
	void UpdateShotCounter() {

		counter += Time.deltaTime;
	}

	void MoveTower() {

		if(Input.GetKey("m")) {
			Debug.Log("-----Pressed M");
			float temp = transform.position.x;
			temp += 1f;
			transform.position = new Vector3(300f, 5f, 180f);
			//transform.position.x = temp;
		}
	}
	
	protected override void CheckIfDestroyed() {
		if(this.health <= 0) {
			print("----TowerObject.CheckIfDestroyed");
			PhotonView photonView = GetComponent<PhotonView>();

			photonView.RPC ("DestroySceneObject", PhotonTargets.MasterClient);
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateShotCounter ();
		MoveTower ();
	}

	#region RPCs
	[RPC]
	void DestroyTower(PhotonMessageInfo info) {
		print ("----DestroyTower RPC");
		if(GetComponent<PhotonView>().viewID == info.photonView.viewID) {
			Destroy (gameObject);
			// Insert explosion here
		}
	}
	#endregion

}
