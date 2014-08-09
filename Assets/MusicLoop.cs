using UnityEngine;
using System.Collections;

public class MusicLoop : MonoBehaviour {

	private string level;
	
	void Awake(){
		// If the scene is new, set the object to remain.
		// According to the intertubes this should work. It doesn't. -_-
		if (Application.loadedLevelName != level){
			level = Application.loadedLevelName;
			DontDestroyOnLoad(transform);
			print("Music shall remain...");
		}
	}
}
