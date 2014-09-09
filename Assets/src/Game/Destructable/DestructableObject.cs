using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour, IDestructable {

	#region Inheritable Members
	public int maxHealth;
	protected int health;
	protected int healthRegen;

	public int Health {
		get{ return this.health; }
		set{
			if(value > this.maxHealth) {
				this.health = this.maxHealth;
			} else {
				this.health = value;
				CheckIfDestroyed();
			}
		}
	}
	
	private void RegenHealth(){
		
		if (this.health < this.maxHealth){
			this.health += this.healthRegen;
			if (this.health > this.maxHealth){
				this.health = this.maxHealth;
			}
		}
	}
	
	[RPC]
	protected void SetColor(Vector3 color){
		
		this.transform.renderer.material.color = new Color(color.x, color.y, color.z);
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
