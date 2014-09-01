using UnityEngine;
using System.Collections;

public class Character : PlayerObject {

	void Start () {
	
		base.Start();
	}
	
	void Update () {
	
		base.Update();
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
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
