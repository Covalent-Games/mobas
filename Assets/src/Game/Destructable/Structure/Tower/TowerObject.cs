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
		this.Damage = 40;
		SetRadius(10);
		SetName();
		RPCSendInitial();
	}

	void Shoot(int newHealth, PhotonView targetPhotonView) {

		PhotonView.Get(this).RPC ("TowerParticleShoot", PhotonTargets.All);
		//Had to use target's photonview because it complained that it couldn't find DealDamage()
		var info = new Dictionary<int, object>();
		info.Add(GameEventParameter.Health, newHealth);
		targetPhotonView.RPC ("UpdateInfo", PhotonTargets.All, info);
		print("Master shot");
	}

	int GetDamage(GameObject target) {

		//TODO: Include other variables in damage calculation
		int newHealth = target.GetComponent<DestructableObject>().Health - Damage;
		return newHealth;
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
		int newHealth = GetDamage(target);


		Vector3 towerVector = transform.position;
		Vector3 playerVector = target.transform.position;
		Debug.DrawLine (towerVector, playerVector, Color.red, 0.25f);
		Shoot (newHealth, PhotonView.Get(target));
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
