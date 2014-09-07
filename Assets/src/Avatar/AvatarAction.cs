using UnityEngine;
using System.Collections;

public class AvatarAction : MonoBehaviour {

	AvatarAttributes avatarAttributes;
	[SerializeField]
	PhotonView photonView;

	[SerializeField]
	private float rateOfFire;
	private float shotDelay;
	private float shotTimer;
	
	private Vector3 lastGunAimPos;

	[SerializeField]
	public int damage;

	// Use this for initialization
	void Start () {

		avatarAttributes = GetComponent<AvatarAttributes>();
		this.shotTimer = 0f;
	}

	/// <summary>
	/// Checks the hit target.
	/// </summary>
	/// <param name="target">Target.</param>
	private void CheckHitTarget(Transform target){
		
		PhotonView view = target.root.GetComponent<PhotonView>();
		if (target.tag == "Structure") {
			//FIXME: how to call RPC through master
			
			//photonView.RPC ("DealDamageToStructure", PhotonTargets.MasterClient, this.damage, target.gameObject.GetInstanceID());
			photonView.RPC ("DealDamageToStructure", PhotonTargets.MasterClient, this.damage, target.GetComponent<PhotonView>().viewID);

		} else if (view != null){
			view.RPC("DealDamage", PhotonTargets.All, this.damage, view.owner.ID);
		}
	}	

	private void ShootIfShooting(){
		
		if (Input.GetButton("Fire1")){
			if (this.shotTimer >= this.shotDelay){
				audio.Play();
				this.shotTimer = 0.0f;
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

		this.shotTimer += Time.deltaTime;
		//Debug.Log(string.Format("ShotTimer: {0} | deltaTime {1}", this.shotTimer, Time.deltaTime));
		//Debug.Log(this.shotDelay);
	}

	
	[RPC]
	public void DealDamage(int damageDealt, int ID){

		if (ID == photonView.owner.ID){

			GetComponent<PlayerObject>().Health -= damageDealt;
		}
	}

	// Update is called once per frame
	void Update () {
		//TODO Put this somewhere safe...
		this.shotDelay = 1f/this.rateOfFire;
		//Debug.Log(this.rateOfFire);
		ShootIfShooting ();
		UpdateFireRate ();
	}

}
