﻿using UnityEngine;
using System.Collections;

public class EscapeMenu: MonoBehaviour {

	// TODO: This should be set globally and applied to all menues
	[SerializeField]
	public int menuWidth = 300;
	[SerializeField]
	public int menueHeight = 300;
	private GUIHandler handler;

	static Rect menuBoxRect;

	void Start(){
		
		handler = GameObject.Find("GuiHandlerObject").GetComponent<GUIHandler>();
		menuBoxRect = new Rect(
				Screen.width/2 - menuWidth/2,
		        Screen.height/2 - menueHeight/2,
		        menuWidth, menueHeight);
	}

	private void ResumeGame(){
		
		handler.CloseCurrentMenu();
	}
	
	private void LogOutToLobby(){
		
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
		PhotonNetwork.LeaveRoom();
	}
	
	void OnLeftRoom(){
		
		Application.LoadLevel("mainMenu");
	}
	
	private void QuitGame(){
		Application.Quit();
	}

	public void DrawMenu(){
		
		// Disable mouselook controls while player is in a menu
		handler.TogglePlayerControls(false);
		
		// Draw bounding box (this is just for looksies)
		GUI.Box(menuBoxRect, "");
		GUILayout.BeginArea(menuBoxRect);
		if (GUILayout.Button("Get Back In The Fight!", GUILayout.MinHeight(50))){
			ResumeGame();
		}
		GUILayout.Space(7);
		if (GUILayout.Button("Abandon Your Team.", GUILayout.MinHeight(50))){
			LogOutToLobby();
		}
		GUILayout.Space(7);
		if (GUILayout.Button("Back Through The Rift...", GUILayout.MinHeight(50))){
			QuitGame();
		}
		GUILayout.EndArea();
		
	}
}
