using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	private GameObject[] towerSpawnPoints;
	
	// Use this for initialization
	public void Begin () {
		print ("Spawn towers");
		towerSpawnPoints = GameObject.FindGameObjectsWithTag("TowerPlaceholder");		
		if(PhotonNetwork.isMasterClient) {
			SpawnTowers ();
		}
	}
	
	void SpawnTowers(){
		
		//print ("Spawning " + towerSpawnPoints.Length + " towers");
		foreach (GameObject spawnPoint in towerSpawnPoints){
			//print ("Tower location: " + spawnPoint.transform.position);
			GameObject tower = PhotonNetwork.Instantiate(
				"Tower",
				spawnPoint.transform.position,
				spawnPoint.transform.rotation,
				0);
			tower.GetComponent<TowerObject>().enabled = true;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnLevelWasLoaded(){
		
		if (GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>().isMaster){
			GameObject.FindGameObjectWithTag("MainCamera").camera.enabled = false;
			GameObject.FindGameObjectWithTag("ServerOverviewCam").camera.enabled = true;
		}
	}
}
