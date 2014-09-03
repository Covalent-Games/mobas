using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour {

	#region Inheritable Members
	protected int maxHealth;
	protected int health;

	public int Health {
		get{ return this.health; }
		set{
			if (this.health + value > this.maxHealth){
				this.health = this.maxHealth;
			} else if(this.health - value <= 0){
				this.health = 0;
				CheckIfDestroyed();
			}
		}
	}
	
	#endregion
	
	#region Inheritable Methods
<<<<<<< HEAD
	protected /*override*/ void CheckIfDestroyed(){
=======
	protected void CheckIfDestroyed(){
>>>>>>> 11fe3489d9928e882dab3c8574413e7dea4a2d3a
		//TODO Destroy logic in case 0 health doesn't actually mean 'dead'
		Destroy(gameObject);
	}
	#endregion
}
