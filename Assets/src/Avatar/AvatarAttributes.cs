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
	
	/// <summary>
	/// Creates an item and places it in the specified location.
	/// </summary>
	/// <param name="itemToLoad">Item to load.</param>
	/// <param name="targetLocation">Target location.</param>
	[RPC]
	void SetItemLocation(string itemToLoad, string targetLocation){
		
		photonView = gameObject.GetComponent<PhotonView>();

		GameObject weapon = (GameObject)Instantiate(
			(GameObject)Resources.Load("Models/"+itemToLoad, typeof(GameObject)));
		Transform weaponSlot = photonView.transform.Find(targetLocation).transform;
		weapon.transform.parent = weaponSlot;
		weapon.transform.position = weaponSlot.position;
		weapon.transform.rotation = weaponSlot.rotation;
	}
}
