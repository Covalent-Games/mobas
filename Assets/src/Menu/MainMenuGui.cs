using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour {

	[SerializeField]
	GUISkin mainMenuSkin;
	Rect _buttonLayoutRect = new Rect(30, 150, 350, 500);


	void LoginGUI(){
		
		if (Input.GetKeyDown(KeyCode.Return)){
			User.Authenticate();
			Debug.Log("ENTER");
		} else {
			GUILayout.BeginArea(_buttonLayoutRect);
			GUILayout.Label("Username", "label");
			User.tmpName = GUILayout.TextField(User.tmpName);
			GUILayout.Label("Password", "label");
			User.tmpPassword = GUILayout.PasswordField(User.tmpPassword, "+"[0], 30);
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Log in", "button")){
					User.Authenticate();
				}
				if (GUILayout.Button("Quit", "button")){
					Application.Quit();
				}
				GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}
	
	/// <summary>
	/// Displays connection information during log in.
	/// </summary>
	void ConnectingToServerGUI(){
	
		GUI.Label(new Rect(50, 50, Screen.width-50, 50),
				string.Format("Connecting to server as {0}", User.Username));
		GUI.Label(new Rect(50, 75, Screen.width-50, 50), PhotonNetwork.connectionStateDetailed.ToString());
	}
	/// <summary>
	/// Displays the Main Menu GUI.
	/// </summary>
	void OpeningMenuGUI(){
		
		if (PhotonNetwork.connectedAndReady){
			GUILayout.BeginArea(_buttonLayoutRect);
			
			if (GUILayout.Button("Enter The Rift", "button")){
				Application.LoadLevel("main");	
			}
			if (GUILayout.Button("Log Out", "button")){
				GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>().DisconnectFromServer();
			}
			if (GUILayout.Button("Quit", "button")){
				Application.Quit();
			}
			GUILayout.EndArea();
			GUI.Label(new Rect(Screen.width/2, 10, 200, 200), string.Format("Name: {0}", User.Username));
		}
	}
	
	void OnGUI(){
	
		GUI.skin = mainMenuSkin;

		switch (User.currentState){
			case User.State.Unidentified:
				LoginGUI();
				break;
			case User.State.Authenticated:
				ConnectingToServerGUI();
				break;
			case User.State.ConnectedToMaster:
				OpeningMenuGUI();
				break;
		}
	}
}