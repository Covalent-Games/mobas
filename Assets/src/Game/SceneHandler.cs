using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	private GameObject[] towerSpawnPoints;
	
	public void Begin () {
		print ("Spawn towers");
		towerSpawnPoints = GameObject.FindGameObjectsWithTag("TowerPlaceholder");		
		if(PhotonNetwork.isMasterClient) {
			SpawnTowers ();
		}
	}
	
	void SpawnTowers(){
		
		foreach (GameObject spawnPoint in towerSpawnPoints){
			GameObject tower = PhotonNetwork.Instantiate(
				"Tower",
				spawnPoint.transform.position,
				spawnPoint.transform.rotation,
				0);
			tower.GetComponent<TowerObject>().enabled = true;
		}
	}
	
	void OnLevelWasLoaded(){
		
		NetworkManager networkManager = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();

		if (networkManager.isMaster){
			GameObject.FindGameObjectWithTag("MainCamera").camera.enabled = false;
			GameObject.FindGameObjectWithTag("ServerOverviewCam").camera.enabled = true;
		}
	}
}
