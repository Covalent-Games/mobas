using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoShooting : MonoBehaviour {

	[SerializeField]
	PhotonView photonView;
	[SerializeField]
	public int maxHealth;
	public int health;
	
	[SerializeField]
	private int healthRegen;
	
	[SerializeField]
	private float rateOfFire;
	private float shotDelay;
	[SerializeField]
	public int damage;
	private Vector3 lastGunAimPos;
	private Transform camPivot;
	[SerializeField]

	

	void Start () {
	
		this.health = this.maxHealth;
		this.rateOfFire /= 60.0f;
		this.shotDelay = this.rateOfFire;
		//TODO: Health regen doens't work properly without health sync across clients
		InvokeRepeating("RegenHealth", 1, 1.0f);
	}

	private void RegenHealth(){
		
		if (this.health < this.maxHealth){
			this.health += this.healthRegen;
			if (this.health > this.maxHealth){
				this.health = this.maxHealth;
			}
		}
	}
	
	private void DeathCheck(){
		if (this.health <= 0){
			transform.position = GameObject.Find("SpawnPoint").transform.position;
			this.health = this.maxHealth;
		}
	}
	
	[RPC]
	public void DealDamage(int damage, int ID){
		
		if (ID == photonView.owner.ID){
			// You just got shot
			this.health -= damage;
			DeathCheck();
		} else {
			// Someone else got shot
			// Update health of player that took damage
		}
	}
	
	private void ShootIfShooting(){
		
		if (Input.GetButton("Fire1")){
			if (this.shotDelay == this.rateOfFire){
				audio.Play();
				this.shotDelay = 0.0f;
				Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
				RaycastHit hitInfo;
				if (Physics.Raycast(mouseRay, out hitInfo)){
					CheckHitTarget(hitInfo.transform);
				}
				this.camPivot = transform.Find("CameraPivot");
				this.lastGunAimPos = camPivot.localEulerAngles;
				this.camPivot.Rotate(new Vector3(-Random.Range(0.5f, 1.0f), 0, 0));
			}
		}
	}
	
	private void CheckHitTarget(Transform target){
		
		PhotonView view = target.root.GetComponent<PhotonView>();
		if (view != null){
			view.RPC("DealDamage", PhotonTargets.All, this.damage, view.owner.ID);
		}
	}	
	
	private void UpdateFireRate(){
		if (this.shotDelay < this.rateOfFire){
			this.shotDelay += Time.deltaTime;
		} else {
			this.shotDelay = this.rateOfFire;
		}
	}
	
	private void GetTargetInfo(){
		
		Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit targetInfo;
		if (Physics.Raycast(mouseRay, out targetInfo)){
			DemoShooting info = targetInfo.transform.GetComponent<DemoShooting>();
			if (info != null){
				string curHealth = string.Format("Health: {0}", info.health);
				GUI.Box(new Rect(Screen.width/2-50, 30, 100, 20), curHealth);
				GUI.Box(new Rect(Screen.width/2-info.health/2, 60, info.health, 20), "");
			}
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(this.health);
		} else {
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
			this.health = (int)stream.ReceiveNext();
		}
	}
	
	void Update () {

		ShootIfShooting();
		UpdateFireRate();
	}
	
	void OnGUI(){

		string curHealth = string.Format("Health: {0}/{1}", this.health, this.maxHealth);
		GUI.Box(new Rect(Screen.width/2-50, Screen.height-50, 100, 20), curHealth);
		GUI.Box(new Rect(Screen.width/2-this.health/2, Screen.height-30, this.health, 20), "");
		GetTargetInfo();
	}
}
