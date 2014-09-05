using UnityEngine;
using System.Collections;

public class Character : PlayerObject {

	[SerializeField]
	PhotonView photonView;
	
	void Start () {
	
		name = gameObject.GetInstanceID().ToString();
		base.Start();
		if(PhotonNetwork.isMasterClient) {
			gameObject.AddComponent<MasterRpcList>();
		}
	}
	
	void Update () {
	
		base.Update();
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo){
		
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
