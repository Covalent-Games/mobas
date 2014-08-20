using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class contains functions for setting the players appearance, weapon, or other such attributes.
/// </summary>
public class AvatarAttributes : MonoBehaviour {

	PhotonView photonView;
	//equipmentList<item, location> should be on the server
	public Dictionary<string, string> equipmentList = new Dictionary<string, string>();
	
	/// <summary>
	/// Sets the color.
	/// </summary>
	/// <param name="color">Color.</param>
	[RPC]
	void SetColor(Vector3 color){
		
		this.transform.renderer.material.color = new Color(color.x, color.y, color.z);
	}
	
	/// <summary>
	/// Loads passed item to passed location, removes item in existing location 
	/// if it exists or moves existing item to passed location.
	/// </summary>
	/// <param name="itemToLoad">Item to load.</param>
	/// <param name="targetLocation">Target location.</param>
	[RPC]
	void SetItemLocation(string itemToLoad, string targetLocation){

		// Get the avatar's photonview to perform the call on
		photonView = gameObject.GetComponent<PhotonView>();
		// Get their equipment "list" -- this will want to be stored on the server eventually
		Dictionary<string, string> currentEquipment = photonView.GetComponent<AvatarAttributes>().equipmentList;
		Transform newItem;
		
		// If the location(value) is already occupied, remove the item(key) in it. 
		// TODO make this able to move the item to an unoccupied slot if possible instead of destroying it
		if (currentEquipment.ContainsValue(targetLocation)){
			foreach (string item in currentEquipment.Keys){
				if (currentEquipment[item] == targetLocation){
					Destroy(photonView.transform.FindChild(targetLocation).FindChild(
						    item+"(Clone)").gameObject);
					currentEquipment.Remove(item);
					break;
				}
			}
		}
		// If we already have this item, reference the existing one instead of making another
		if (currentEquipment.ContainsKey(itemToLoad)){
			newItem = photonView.transform.FindChild(currentEquipment[itemToLoad]).FindChild(itemToLoad+"(Clone)");
			// Since we're moving the item we'll remove the entry in the avatars equipment list
			currentEquipment.Remove(itemToLoad);
		// The weapon doesn't already exist so we create it
		}else{
			// Note: the instantiated item will have the string "(Clone)" attached to its name
			newItem = (Transform)Instantiate(
				(Transform)Resources.Load("Models/"+itemToLoad, typeof(Transform)));
		}
		currentEquipment.Add(itemToLoad, targetLocation);
		// Get the weapon slot and parent the weapon to it and set its position and rotation to match
		Transform weaponSlot = photonView.transform.Find(targetLocation).transform;
		newItem.transform.parent = weaponSlot;
		newItem.transform.position = weaponSlot.position;
		newItem.transform.rotation = weaponSlot.rotation;		
	}
}
