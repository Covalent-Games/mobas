﻿using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {


	GameObject player;
	Vector3 spawnPoint;
	RoomOptions newRoomDetails;
	
	void Start () {
		
		setRoomOptions();
		JoinRoom();
		
	}
	void OnJoinedRoom(){

		Debug.Log("Player joined room");
		SpawnPlayer();
		EnableLocalControl();
	}
	void setRoomOptions(){

				newRoomDetails = new RoomOptions ();
	}
	
	void JoinRoom(){

		PhotonNetwork.JoinOrCreateRoom("Yeeha!",
		                               newRoomDetails,
		                               TypedLobby.Default);
	}	
	void SpawnPlayer(){

		spawnPoint = GameObject.Find("SpawnPoint").transform.position;
		player = (GameObject)PhotonNetwork.Instantiate (
			"Player",
			spawnPoint, 
			Quaternion.identity,
			0);
	}
	
	void EnableLocalControl(){

		PhotonView pv = player.GetComponent<PhotonView>();
		Transform cameraPivot = player.transform.Find("CameraPivot");

		if (pv.isMine){
			player.GetComponent<AvatarMovement>().enabled = true;
			cameraPivot.GetComponent<CameraXPivot>().enabled = true;
			Camera camera = GameObject.Find("MainCamera").camera;
			camera.transform.parent = cameraPivot;
			Vector3 pos = new Vector3(1.0f, 1.0f, -3.5f);
			camera.transform.position = cameraPivot.transform.position + pos;
			player.gameObject.tag = "Player";
		}
	}
}
