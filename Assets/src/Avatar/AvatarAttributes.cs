using UnityEngine;
using System.Collections;

/// <summary>
/// This class contains functions for setting the players appearance, weapon, or other such attributes.
/// </summary>
public class AvatarAttributes : MonoBehaviour {

	PhotonView photonView;
	
	
	[RPC]
	void SetColor(Vector3 color){
		
		this.transform.renderer.material.color = new Color(color.x, color.y, color.z);
	}
	
	[RPC]
	void SetWeapon(string gunToLoad){
		
		photonView = gameObject.GetComponent<PhotonView>();

		GameObject weapon = (GameObject)Instantiate(
			(GameObject)Resources.Load("Models/"+gunToLoad, typeof(GameObject)));
		Transform weaponSlot = photonView.transform.Find("EquippedWeapon").transform;
		weapon.transform.parent = weaponSlot;
		weapon.transform.position = weaponSlot.position;
		weapon.transform.rotation = weaponSlot.rotation;
	}

}
