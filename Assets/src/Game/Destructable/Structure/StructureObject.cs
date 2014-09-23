using UnityEngine;
using System.Collections;

public class StructureObject : DestructableObject {

	//protected float radiusOfEffect;

	public void SetRadius(int radius) {
		
		//transform.Find("VisionCollider").GetComponent<CapsuleCollider>().radius = 15;
	}

	protected void SetName() {

		name = name + gameObject.GetPhotonView().viewID.ToString();
	}
}
