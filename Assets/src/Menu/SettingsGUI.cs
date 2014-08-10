using UnityEngine;
using System.Collections;

public class SettingsGUI : MonoBehaviour {
	
	int width;
	int height;
	
	void Start(){
		
		width = 700;
		height = 600;
	}

	public void DrawMenu(){

		GUI.Box(new Rect(Screen.width/2-width/2, 150, width, height), "This will be settings");
	}
}
