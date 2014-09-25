using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableObject : MonoBehaviour {

	#region Inheritable Members
	public int maxHealth;
	protected int health;
	protected int healthRegen;
	public Faction faction;

	// Attributes
	public int targetDamage = 0;
	public int areaDamage = 0;
	public int defence = 0;
	public int healing = 0;

	public int Health {
		get{ return this.health; }
		set{
			if( value > this.maxHealth ) {
				this.health = this.maxHealth;
			} else {
				this.health = value;
				if ( this.health <= 0 && PhotonNetwork.isMasterClient ){
					EndObject();
				}
			}
		}
	}
	
	protected void RegenHealth(){
		
		this.Health += this.healthRegen;
	}
	
	public void RPCSendInitial(){
		
		var info = new Dictionary<int, object>();
		info.Add(GameEventParameter.Health, this.Health);
		PhotonView.Get(this).RPC("UpdateInfo", PhotonTargets.AllBuffered, info);
	}

	public int CalculateDamage(int health, int defence) {
		
		int newHealth;
		float finalDamage = this.targetDamage * ((100-defence) / 100.0f);
		Debug.Log("Damage after defence: " + finalDamage);
		newHealth = health - Mathf.RoundToInt(finalDamage);
		return newHealth;
	}
	
	[RPC]
	protected void SetColor(float[] color){
		
		this.transform.renderer.material.color = new Color(color[0], color[1], color[2]);
	}


	#endregion

	#region Inheritable Methods
	protected virtual void EndObject() {

		PhotonNetwork.Destroy (gameObject);
	}
	#endregion

	[RPC]
	public void UpdateInfo(Dictionary<int, object> info, PhotonMessageInfo senderInfo) {
		
		// Trigger graphic gore in the mouth that makes babies cry
		if(PhotonView.Get(this).viewID == senderInfo.photonView.viewID) {
			this.Health = (int)info[GameEventParameter.Health];
		}
	}

	//TODO: Remove -- No longer being used
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
