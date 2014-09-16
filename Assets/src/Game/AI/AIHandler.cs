using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIHandler : MonoBehaviour {

	private GameObject[] creepSpawnPoints;
	//HACK
	float spawnDelay = 10f;
	float spawnDelayTimer = 10f;
	public int minionsToSpawn = 3;
	

	// Use this for initialization
	void Start () {
	
		creepSpawnPoints = GameObject.FindGameObjectsWithTag("CreepSpawnPoint");	
	}
	
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
	
	// Update is called once per frame
	void Update () {
	
		if (PhotonNetwork.isMasterClient == false){return;}
		if (UpdateSpawnTimer()){
			SpawnCreeps();
		}
	}
}
