using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.Lite; /*
	ReceiverGroup
*/

public class TestHeroAction : MonoBehaviour, IActions {

	[SerializeField]
	public float rateOfFire;
	private float shotDelay;
	private float shotTimer;
	
	private Vector3 lastGunAimPos;
		
	public float RateOfFire{ 
		get{return rateOfFire;}
		set{rateOfFire = value;}}

	// Use this for initialization
	public void Start () {

		this.shotTimer = 0f;
		
		SetUpAttributes();
		
		//TODO: Move this to the proper script once items are coded in.
		EquipWeapon();
		
		if (PhotonNetwork.isMasterClient){
			this.enabled = false;
		}
	}

	void SetUpAttributes() {

		Debug.Log("---SetUpAttributes for " + this.gameObject.GetPhotonView().viewID);
		PlayerObject player = gameObject.GetComponent<PlayerObject>();
		player.Health = 500;
		player.defence = 15;
		player.targetDamage = 10;
		player.areaDamage = 8;
		player.healing = 20;
	}
	
	void EquipWeapon(){
	
		Transform newItem = 
			(Transform)Instantiate((Transform)Resources.Load("Models/test_handgun", typeof(Transform)));
		Transform slot = transform.FindChild("ReadiedItem");
		if (slot != null){
			newItem.parent = slot;
			newItem.position = slot.position;
		} else {
			Debug.LogError("No slot found for item equipping");
		}
	}

	public void PrimaryAction(){
		
		if (this.shotTimer >= this.shotDelay){
			audio.Play();
			//TODO: Sync shot timer with server/verify somehow shooting is allowed
			this.shotTimer = 0.0f;
			Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hitInfo;
			if (Physics.Raycast(mouseRay, out hitInfo)){
			
				PhotonView targetView = PhotonView.Get(hitInfo.transform.gameObject);
				if (targetView != null){
				
					RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
					raiseEventOptions.Receivers = (ReceiverGroup)PhotonTargets.MasterClient;
					
					var parameters = new Dictionary<int, object>();
					parameters.Add(GameEventParameter.TargetViewID, targetView.viewID);
					parameters.Add(GameEventParameter.SenderViewID, PhotonView.Get(this).viewID);
					
					print ("local damage before event: " + this.GetComponent<DestructableObject>().targetDamage);
				
					if (!PhotonNetwork.networkingPeer.OpRaiseEvent((byte)GameEventCode.PrimaryAction, parameters, true, raiseEventOptions)){
						Debug.LogWarning("PrimaryAction event was unable to send!");
					}
				}
			}
			Transform camera = transform.Find("MainCamera");
			camera.RotateAround(transform.position, transform.right, -1.0f);
			this.lastGunAimPos = camera.localEulerAngles;
		}

	}
	
	public bool ActionOne(){
		
		Debug.Log("Using Skill 1!");
		return true;
	}
	
	public bool ActionTwo(){
		
		Debug.Log("Using Skill 2!");
		return true;
	}

	public bool ActionThree(){
		
		Debug.Log("Using Skill 3!");
		return true;
	}
	
	public bool ActionFour(){
		
		Debug.Log("Using Skill 4!");
		return true;
	}

	private void UpdateFireRate(){

		//if (this.shotTimer < this.shotDelay){
			this.shotTimer += Time.deltaTime;
			// Update shot delay in case rate of fire changed (likely due to weapon switch or ability)
			this.shotDelay = 1f/this.rateOfFire;
		//}
	}
	
	public void UpdateGlobalCooldown(PlayerObject player){
		
		player.globalCooldownTimer += Time.deltaTime;
	}

	public void Update () {
	
		//TODO Don't get this component every frame... setting it right now as a member doesn't work though (via Start())
		PlayerObject player = GetComponent<PlayerObject>();
		//Actions
		if (player.primaryActionEnabled){
			if (Input.GetButton("Fire1")){
				PrimaryAction();
			}
		}
		if (player.actionsEnabled && player.globalCooldownTimer > player.globalCooldown){
			bool successfulAction;
			if (Input.GetKeyDown(KeyCode.Alpha1)){
				successfulAction = ActionOne();
			} else if (Input.GetKeyDown(KeyCode.Alpha2)){
				successfulAction = ActionTwo();
			} else if (Input.GetKeyDown(KeyCode.Alpha3)){
				successfulAction = ActionThree();
			} else if (Input.GetKeyDown(KeyCode.Alpha4)){
				successfulAction = ActionFour();
			} else {successfulAction = false;}
			
			if (successfulAction){
				player.globalCooldownTimer = 0f;
			}
		}
		
		UpdateFireRate();
		UpdateGlobalCooldown(player);
	}
}
