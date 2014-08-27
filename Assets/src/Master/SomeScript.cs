using UnityEngine;
using System.Collections;

public class SomeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	[RPC]
	public void TestFunc(PhotonMessageInfo info) {
		Debug.Log("sender: " + info.sender +
		          "\nID: " + info.photonView.owner.ID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
