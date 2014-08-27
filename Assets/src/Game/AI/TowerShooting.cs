using UnityEngine;
using System.Collections;

public class TowerShooting : MonoBehaviour {

	public int maxHealth;
	public int health;

	public float radiusOfEffect;
	public int rateOfFire;
	public int damage;

	// Use this for initialization
	void Start () {
		gameObject.name = gameObject.GetInstanceID ().ToString();
		this.damage = 8;
		this.health = this.maxHealth;
	}

	public void DeathCheck(){
		if (this.health <= 0){
			Destroy(gameObject);
		}
	}
	
	public void TakeDamage(int damage) {
		this.health -= damage;
		DeathCheck ();
	}

	public void CheckForPlayer() {

	}

	// Update is called once per frame
	void Update () {
		CheckForPlayer ();
	}
}
