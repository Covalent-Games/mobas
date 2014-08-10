using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	void Awake(){
		// This doesn't work, sadly
		DontDestroyOnLoad(transform.gameObject);
		Debug.Log("Music SHOULD stick around...");
	}
}
