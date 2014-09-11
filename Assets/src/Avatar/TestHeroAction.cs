﻿using UnityEngine;
using System.Collections;

public class TestHeroAction : MonoBehaviour, IActions {

	[SerializeField]
	private float rateOfFire;
	private float shotDelay;
	private float shotTimer;
	
	private Vector3 lastGunAimPos;

	[SerializeField]
	public int damage;
		

	// Use this for initialization
	void Start () {

		this.shotTimer = 0f;
	}

	/// <summary>
	/// Checks the hit target.
	/// </summary>
	/// <param name="target">Target.</param>
	private void CheckHitTarget(Transform target){
		
		PhotonView view = target.root.GetComponent<PhotonView>();
		/*
		 * if (view.gameObject.tag == "Structure") {
			view.RPC ("DealDamageToStructure",
			          PhotonTargets.MasterClient,
			          this.damage,
			          target.gameObject.GetInstanceID());
		} else if(view.gameObject.tag == "Player") {
			view.RPC ("DealDamage", PhotonTargets.All, this.damage, view.owner.ID);
		} else if(view != null) {

		}
		*/


		if (view != null){
			gameObject.GetPhotonView().RPC("DealDamage", PhotonTargets.MasterClient, this.damage, view.viewID);
		}
		/*else if (target.tag == "Structure") {
			view.RPC("DealDamageToMobile", PhotonTargets.MasterClient, this.damage);
		} else if (target.tag == "Structure") {
			//FIXME: how to call RPC through master

			GetComponent<PhotonView>().RPC (
				"DealDamageToStructure",
				PhotonTargets.MasterClient,
				this.damage,
				target.gameObject.GetInstanceID());
		}
		*/
	}	

	public void PrimaryAction(){
		
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
