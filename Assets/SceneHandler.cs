using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	List<SceneObjectOptions> sceneObjects = new List<SceneObjectOptions>();

	// Use this for initialization
	public void BuildScene () {

		print ("---Scene Handler OnJoinedRoom");
		BuildSceneObjectDictionary ();
		InstantiateSceneObjects ();
	}

	void BuildSceneObjectDictionary() {

		AddTowersToDictionary ();
	}

	void AddTowersToDictionary() {

		print ("---AddTowersToDictionary");
		GameObject[] placeholders = GameObject.FindGameObjectsWithTag ("TowerPlaceholder");
		foreach (GameObject placeholder in placeholders) {
			this.sceneObjects.Add(new SceneObjectOptions("Tower",
			                                             placeholder.transform.position, 
			                                             placeholder.transform.rotation,
			                                             (int)DestructableObject.groupID.towers));
		}
	}

	void InstantiateSceneObjects() {

		foreach(SceneObjectOptions sceneObjectData in sceneObjects) {
			print ("name: " + sceneObjectData.prefabName + ", position: " + sceneObjectData.position + ", rotation: " + sceneObjectData.rotation + ", group: " + sceneObjectData.group);

			GameObject random = (GameObject)PhotonNetwork.InstantiateSceneObject(sceneObjectData.prefabName,
			                                     sceneObjectData.position,
			                                     sceneObjectData.rotation,
			                                     sceneObjectData.group,
			                                     null);
			print (random);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class SceneObjectOptions {

	public string prefabName;
	public Vector3 position;
	public Quaternion rotation;
	public int group;

	public SceneObjectOptions(string name, Vector3 pos, Quaternion rot, int g) {

		prefabName = name;
		position = pos;
		rotation = rot;
		group = g;
	}

}
