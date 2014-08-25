using UnityEngine;
using System.Collections;

public class AvatarAction : MonoBehaviour {

	AvatarAttributes avatarAttributes;
	PhotonView photonViewObject;

	[SerializeField]
	private float rateOfFire;
	private float shotDelay;
	
	private Vector3 lastGunAimPos;

	[SerializeField]
	public int damage;

	// Use this for initialization
	void Start () {
		avatarAttributes = GetComponent<AvatarAttributes>();
		photonViewObject = GetComponent<PhotonView> ();
		Debug.Log ("***************Ran Start()");
		if (!photonViewObject)
						Debug.Log ("*************photonView is null!");
		this.rateOfFire /= 60.0f;
		this.shotDelay = this.rateOfFire;
		this.damage = 5;
	}

	/// <summary>
	/// Checks the hit target.
	/// </summary>
	/// <param name="target">Target.</param>
	private void CheckHitTarget(Transform target){
		
		PhotonView view = target.root.GetComponent<PhotonView>();
		if (view != null){
			view.RPC("DealDamage", PhotonTargets.All, this.damage, view.owner.ID);
		}
	}	

	private void ShootIfShooting(){
		
		if (Input.GetButton("Fire1")){
			if (this.shotDelay == this.rateOfFire){
				audio.Play();
				this.shotDelay = 0.0f;
				Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
				RaycastHit hitInfo;
				if (Physics.Raycast(mouseRay, out hitInfo)){
					CheckHitTarget(hitInfo.transform);
				}
				Transform camera = transform.Find("MainCamera");
				camera.RotateAround(transform.position, transform.right, -1.0f);
				this.lastGunAimPos = camera.localEulerAngles;
			}
		}
	}

	private void UpdateFireRate(){
		if (this.shotDelay < this.rateOfFire){
			this.shotDelay += Time.deltaTime;
		} else {
			this.shotDelay = this.rateOfFire;
		}
	}

	
	[RPC]
	public void DealDamage(int damageDealt, int ID){
		//if (ID == photonViewObject.photonView.owner.ID){

		if (ID == photonViewObject.ownerId){
			// You just got shot

			//avatarAttributes.health -= damageDealt;
			if(!avatarAttributes) {
				Debug.Log("avatarAttributes is null");
			}
			else {
				avatarAttributes.TakeDamage(damageDealt);
				avatarAttributes.DeathCheck();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		ShootIfShooting ();
		UpdateFireRate ();
	}

}
