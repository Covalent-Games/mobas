using UnityEngine;
using System.Collections;

public interface IActions  {

	//float globalCooldown;
	//float globalCooldownTimer;
	float RateOfFire { get; set; }
	void Start();
	//TODO These should be delegates for more flexibility on what each skill can do
	void PrimaryAction();
	bool ActionOne();
	bool ActionTwo();
	bool ActionThree();
	bool ActionFour();

	void Update();
	void UpdateGlobalCooldown(PlayerObject player);
}