using UnityEngine;
using System.Collections;

public interface IActions  {

	//float globalCooldown;
	//float globalCooldownTimer;

	//TODO These should be delegates for more flexibility on what each skill can do
	void PrimaryAction();
	bool ActionOne();
	bool ActionTwo();
	bool ActionThree();
	bool ActionFour();

	void Update();
	void UpdateGlobalCooldown(PlayerObject player);
}