using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {


	void Awake(){ 
	
		DontDestroyOnLoad(this);
	}
	
	public void OnLevelWasLoaded(){
	
		if (PhotonNetwork.isMasterClient){ 
			LoadMasterScripts();
		} else {
			LoadClientScripts();
		}	
	}
	
	void LoadMasterScripts(){
		
		GameObject.FindObjectOfType<SceneHandler>().enabled = true;
	}
	
	void LoadClientScripts(){
	
		GameObject.FindObjectOfType<PlayerHandler>().enabled = true;
	}
}
