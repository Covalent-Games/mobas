using UnityEngine;
using System.Collections;

public class StructureObject : DestructableObject {

	//protected float radiusOfEffect;

	public void SetRadius(int radius) {
		
		GetComponent<CapsuleCollider> ().radius = 15;
	}

	public void Start() {
		//name = gameObject.GetInstanceID ().ToString();
	}

	public void Update() {

	}

}
