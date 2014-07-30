using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void DrawMenu() {
		
		GUI.Box(new Rect(Screen.width-400, Screen.height-400, 300, 500), "Inventory");
	}
}
