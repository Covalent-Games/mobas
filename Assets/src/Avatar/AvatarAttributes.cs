using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class contains functions for setting the players appearance, weapon, or other such attributes.
/// </summary>
public class AvatarAttributes : MonoBehaviour {

	PhotonView photonView;

	[SerializeField]
	public int maxHealth;

	public int health;

	[SerializeField]
	private int healthRegen;

	void Start() {
		this.health = this.maxHealth;
		//TODO: Health regen doens't work properly without health sync across clients
		InvokeRepeating("RegenHealth", 1, 1.0f);
	}

	public void DeathCheck(){
		if (this.health <= 0){
			transform.position = GameObject.Find("SpawnPoint").transform.position;
			this.health = this.maxHealth;
		}
	}

	public void TakeDamage(int damage) {
		this.health -= damage;
	}
	


	private void GetTargetInfo(){
		
		Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit targetInfo;
		if (Physics.Raycast(mouseRay, out targetInfo)){
			AvatarAttributes info = targetInfo.transform.GetComponent<AvatarAttributes>();
			if (info != null){
				string curHealth = string.Format("Health: {0}", info.health);
				GUI.Box(new Rect(Screen.width/2-50, 30, 100, 20), curHealth);
				GUI.Box(new Rect(Screen.width/2-info.health/2, 60, info.health, 20), "");
			} else if(targetInfo.transform.gameObject.tag == "Structure") {
				TowerObject tower = targetInfo.transform.gameObject.GetComponent<TowerObject>();
				string curHealth = string.Format("Health: {0}", tower.Health);
				GUI.Box(new Rect(Screen.width/2-50, 30, 100, 20), curHealth);
				GUI.Box(new Rect(Screen.width/2-tower.Health/2, 60, tower.Health, 20), "");
			}
		}
	}

	void OnGUI(){
		
		string curHealth = string.Format("Health: {0}/{1}", this.health, this.maxHealth);
		GUI.Box(new Rect(Screen.width/2-50, Screen.height-50, 100, 20), curHealth);
		GUI.Box(new Rect(Screen.width/2-this.health/2, Screen.height-30, this.health, 20), "");
		GetTargetInfo();
	}
}
