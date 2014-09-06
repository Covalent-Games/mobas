using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour {

	public enum groupID {
		players,
		towers,
		creeps
	};

	#region Inheritable Members
	protected int maxHealth;
	protected int health;

	public int Health {
		get{ return this.health; }
		set{
			if(value > this.maxHealth) {
				this.health = this.maxHealth;
			} else {
				this.health = value;
				CheckIfDestroyed();
			}
			/*
			if (this.health + value > this.maxHealth){
				this.health = this.maxHealth;
			} else if(this.health - value <= 0){
				this.health = 0;
				CheckIfDestroyed();
			}
			*/
		}
	}
	
	#endregion

	#region Inheritable Methods
	protected virtual void CheckIfDestroyed() {
		if(this.health <= 0) {
			Destroy (gameObject);
		}
	}
	#endregion



}
