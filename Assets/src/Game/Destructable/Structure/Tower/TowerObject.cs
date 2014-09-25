using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerObject : StructureObject {

	float counter = 0f;
	float shotInterval = 2.0f;
	public List<GameObject> targeted = new List<GameObject>();


	// Use this for initialization
	void Start () {
		this.maxHealth = 200;
		this.Health = 200;
		this.targetDamage = 40;
		this.defence = 25;
		SetRadius(10);
		SetName();
		RPCSendInitial();
	}

	void Shoot(GameObject target) {

		PhotonView.Get(this).RPC ("TowerParticleShoot", PhotonTargets.All);
		DestructableObject destructable = target.GetComponent<DestructableObject>();
		int newHealth = CalculateDamage(destructable.Health, destructable.defence);
		//Had to use target's photonview because it complained that it couldn't find DealDamage()
		var info = new Dictionary<int, object>();
		info.Add(GameEventParameter.Health, newHealth);
		PhotonView.Get(target).RPC ("UpdateInfo", PhotonTargets.All, info);
		print("Master shot");
	}

	GameObject AcquireTarget() {

		GameObject getsShot = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach(var target in targeted) {
			//HACK: come up with an elegant way of removing targets outside foreach loop
			if(target != null) {
				Vector3 separation = target.transform.position - position;
				float currentDistance = separation.sqrMagnitude;
				if(currentDistance < distance) {
					getsShot = target;
					distance = currentDistance;
				}
			}
		}

		if (getsShot != null) {
			transform.Find("TurretAxel").LookAt(getsShot.transform);
		}

		return getsShot;
	}

	void ShootSomething() {

		GameObject target = AcquireTarget();
		if(target == null) {
			return;
		}

		Vector3 towerVector = transform.position;
		Vector3 playerVector = target.transform.position;
		Debug.DrawLine (towerVector, playerVector, Color.red, 0.25f);
		Shoot (target);
	}


	void UpdateShotCounter() {

		counter += Time.deltaTime;
		if(counter > this.shotInterval) {
			//TODO: Put this somewhere more efficient
			targeted.RemoveAll(item => item == null);
			if(targeted.Count > 0) {
				ShootSomething();
				counter = 0f;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateShotCounter ();
	}

	[RPC]
	public void TowerParticleShoot(PhotonMessageInfo info) {
		
		if (info.photonView == PhotonView.Get(this)) {
			audio.Play();
			this.gameObject.GetComponentInChildren<ParticleSystem>().Play();
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
		if (stream.isWriting){
			stream.SendNext(transform.FindChild("TurretAxel").transform.rotation);
		} else {
			transform.FindChild("TurretAxel").transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}

}
