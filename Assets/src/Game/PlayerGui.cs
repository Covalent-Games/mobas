using UnityEngine;
using System.Collections;

public class PlayerGui : MonoBehaviour {

	public Texture2D crosshair;
	public float crosshairSize = 10.0f; // Larger number DECREASES size

	float crosshairPosAdjust;
	int screenCenterX;
	int screenCenterY;
	
	void Start () {

		// Center crosshair based on size of image
		crosshairPosAdjust = crosshairSize/2;
		// Get the center of the screen
		screenCenterX = Screen.width/2;
		screenCenterY = Screen.height/2;
	}
	
	protected void DrawCrosshair(){

		GUI.DrawTexture(new Rect(screenCenterX-crosshair.width/crosshairSize,
		                         screenCenterY-crosshair.height/crosshairSize,
		                         crosshair.width/crosshairPosAdjust, crosshair.height/crosshairPosAdjust),
		                crosshair);
		
	}
	
	void OnGUI () {
	
		DrawCrosshair();
			        
	}
}
