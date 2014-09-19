using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {


	void Awake(){ DontDestroyOnLoad(this); }
	
	public void OnLevelWasLoaded(){
	
		if (!PhotonNetwork.isMasterClient){ 
			GameObject.FindObjectOfType<PlayerHandler>().enabled = true;
		}
	
		Debug.Log("Enabling handlers...");
		
		GameObject.FindObjectOfType<SceneHandler>().enabled = true;
	}
}
