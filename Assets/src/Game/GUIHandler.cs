using UnityEngine;
using System.Collections;

public class GUIHandler : MonoBehaviour {

	public Texture2D crosshair;
	public float crosshairSize = 10.0f; // Larger number DECREASES size

	private float crosshairPosAdjust;
	private int screenCenterX;
	private int screenCenterY;
	private Rect crosshairRect;
	
	// Delegate for assigning Menues to methods
	// This allows us to assign a method as if it were a variable.
	public delegate void MenuDelegate();
	public MenuDelegate DrawOpenMenu;
	
	EscapeMenu escapeMenu;
	Inventory inventoryMenu;
		
	void Start () {

		Screen.lockCursor = true;
		SetCrosshairVariables();
		escapeMenu = gameObject.AddComponent<EscapeMenu>();
		inventoryMenu = gameObject.AddComponent<Inventory>();
	}
	
	private void SetCrosshairVariables(){
	
		// Center crosshair based on size of image
		crosshairPosAdjust = crosshairSize/2;
		// Get the center of the screen
		screenCenterX = Screen.width/2;
		screenCenterY = Screen.height/2;
		// Create new Rect for crosshair
		crosshairRect = new Rect(screenCenterX-crosshair.width/crosshairSize,
		                         screenCenterY-crosshair.height/crosshairSize,
		                         crosshair.width/crosshairPosAdjust, crosshair.height/crosshairPosAdjust);
	}
	
	public void TogglePlayerControls(bool toggle){
	
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null){
			player.transform.Find("CameraPivot").GetComponent<CameraXPivot>().enabled = toggle;
			player.GetComponent<AvatarMovement>().enabled = toggle;
		}
	}
	
	private void DrawCrosshair(){

		GUI.DrawTexture(crosshairRect, crosshair);
	}
	
	public void CloseCurrentMenu(){
		
		DrawOpenMenu = null;
		TogglePlayerControls(true);
		Screen.lockCursor = true;
	}
	
	private void CheckForKeyPress(){

		if (Input.GetKeyDown (KeyCode.Escape)){
			if (DrawOpenMenu == null){
				Screen.lockCursor = false;
				DrawOpenMenu = escapeMenu.DrawMenu;
			} else {
				CloseCurrentMenu();
			}
		}
		if (Input.GetKeyDown (KeyCode.I)){
			DrawOpenMenu = inventoryMenu.DrawMenu;
		}
	}
	
	void Update(){
		
		CheckForKeyPress();
	}
	
	void OnGUI () {

		if (DrawOpenMenu != null){ // If null, no menu is open.
			DrawOpenMenu();	
		} else {
			DrawCrosshair();
		}
	}
}
