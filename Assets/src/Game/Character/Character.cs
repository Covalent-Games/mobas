using UnityEngine;
using System.Collections;

public class Character : PlayerObject {

	void Start () {
	
		base.Start();
	}
	
	void Update () {
	
		base.Update();
	}
	
	// Has to be top level or Photon can't see it.
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
	Debug.Log("Sgfjdksjf");
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(this.Health);
		} else {
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
			this.Health = (int)stream.ReceiveNext();
		}
	}
}
