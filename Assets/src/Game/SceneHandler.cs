using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	private GameObject[] towerSpawnPoints;
	private GameObject[] creepSpawnPoints;
	//HACK
	float spawnDelay = 10f;
	float spawnDelayTimer = 10f;
	public int minionsToSpawn = 3;

	
	void SpawnCreeps(){
		
		foreach (GameObject spawnPoint in creepSpawnPoints){
			Vector3 pos = spawnPoint.transform.position;
			for (int count = 0; count < minionsToSpawn; count++){
				Vector2 randomPos = Random.insideUnitCircle * 5;
				pos.x += randomPos.x;
				pos.z += randomPos.y;
				GameObject creep = PhotonNetwork.Instantiate(
					"TestCreep",
					pos,
					spawnPoint.transform.rotation,
					// This needs to be something different, likely.
					0);
				creep.GetComponent<CreepAI>().enabled = true;
			}
		}
	}
	
	bool UpdateSpawnTimer(){
		
		spawnDelayTimer += Time.deltaTime;
		if (spawnDelayTimer > spawnDelay){
			spawnDelayTimer = 0f;
			return true;
		}
		return false;
	}
	public void Begin () {

		towerSpawnPoints = GameObject.FindGameObjectsWithTag("TowerPlaceholder");
		creepSpawnPoints = GameObject.FindGameObjectsWithTag("CreepSpawnPoint");	
		
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
	
	void Update(){
	
		if (UpdateSpawnTimer()){
			SpawnCreeps();
		}
	}
	
	#region RPC
	
	[RPC]
	public void SpawnNewCreep(Vector3 pos, Quaternion rot, string creepType){
	
		
	}
	
	#endregion
}
