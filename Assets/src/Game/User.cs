using UnityEngine;
using System.Collections;

public static class User{

	public enum State {
		Unidentified,
		Authenticated,
		ConnectedToMaster,
		ConnectedToGame};
	public static State currentState = User.State.Unidentified;
	public static string tmpName = "";
	public static string tmpPassword = "";
	private static string username = "";
	
	///<summary>
	/// The name the player logged in with. It cannot be modified unless it is being set to "".
	/// </summary>
	//TODO Move this to server
	public static string Username { 
		get{
			return username;
		} 
		set{
			if (value == ""){
				username = "";
			}
		}
	}
	
	// TODO Return bool as returned from OpAuthenticate
	/// <summary>
	/// Authenticate this instance.
	/// </summary>
	public static void Authenticate(){
		if (User.tmpName == ""){
			Debug.LogWarning("No username provided!");
		}
		if (User.tmpPassword == ""){
			Debug.LogWarning("No password provided!");
		}
		/* This is where we'll trigger the authentication. According 
		* the the Exit Games website, Steam Authentication will be 
		* available soon. Until then we'll just fake it or use a YAML file.*/
		User.currentState = User.State.Authenticated;
		
		GameObject goNetwork = GameObject.FindGameObjectWithTag("Network");
		if (goNetwork != null){
			goNetwork.GetComponent<NetworkManager>().InitiateConnection();
			// This straight up doesn't belong here. :\
			username = tmpName;
			tmpName = "";
			tmpPassword = "";
		} else {
			Debug.LogError("No network GameObject located in the current scene!");
		}
	}
}
