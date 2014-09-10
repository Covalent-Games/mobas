using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	private GameObject[] towerSpawnPoints;
	
	// Use this for initialization
	public void Begin () {
		
		towerSpawnPoints = GameObject.FindGameObjectsWithTag("TowerPlaceholder");		
		if(PhotonNetwork.isMasterClient) {
			SpawnTowers ();
		}
	}
	
	void SpawnTowers(){
		
		//print ("Spawning " + towerSpawnPoints.Length + " towers");
		foreach (GameObject spawnPoint in towerSpawnPoints){
			//print ("Tower location: " + spawnPoint.transform.position);
			PhotonNetwork.Instantiate("Tower",
			                          spawnPoint.transform.position,
			                          spawnPoint.transform.rotation,
			                          0);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
