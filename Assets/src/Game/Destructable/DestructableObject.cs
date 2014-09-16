using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableObject : MonoBehaviour, IDestructable {

	#region Inheritable Members
	public int maxHealth;
	protected int health;
	protected int healthRegen;
	public int faction;
	protected int damage = 1;

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

	public int Damage {
		get{ return this.damage;}
		set{ this.damage = value;}
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
			PhotonNetwork.Destroy (gameObject);
		}
	}
	#endregion

	[RPC]
	public void UpdateInfo(Dictionary<int, object> info, PhotonMessageInfo senderInfo) {
		
		// Trigger graphic gore in the mouth that makes babies cry
		if(PhotonView.Get(this).viewID == senderInfo.photonView.viewID) {
			this.Health = (int)info[GameEventParameter.Health];
		}
	}

	[RPC]
	public void TakeDamage(int damageDealt, int ID, int dealerID, PhotonMessageInfo master){
		print(dealerID + " shot " + ID);
		print ("My viewID: " + GetComponent<PhotonView> ().viewID);
		if (ID == GetComponent<PhotonView>().viewID){
			print(dealerID + " shot " + PhotonView.Find(ID).viewID);
			GetComponent<DestructableObject>().Health -= damageDealt;
		} else {
			if(PhotonView.Find(ID) != null) {
				PhotonView.Find(ID).gameObject.GetComponent<DestructableObject>().Health -= damageDealt;
			} else {
				print ("----Already destroyed");
			}
		}
	}

}
