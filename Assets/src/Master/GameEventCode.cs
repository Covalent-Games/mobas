using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameEventCode{

	PrimaryAction = 1,
	SpawnPlayer,
	MovePlayer,
	RotatePlayer,
}

//TODO: Urgent. Change these to Bytes. They do not have to be unique, and therefore can have more than 255. bytes = 2 bytes, int = 5 bytes.
public class GameEventParameter{
	
	#region Common Parameters | Must be unique
	public const int TargetViewID = 901;
	public const int SenderViewID = 902;
	public const int Health = 903;
	#endregion
	
	#region SpawnPlayer
	public const int CharacterName = 0;
	#endregion
	
	#region MovePlayer
	public const int Horizontal = 0;
	public const int Vertical = 1;
	#endregion
}

public class RPCName {

	public const string ToggleMovement = "ToggleMovement";
	public const string ToggleLook = "ToggleLook";
	public const string TogglePrimaryAction = "TogglePrimaryAction";
	public const string ToggleActions = "ToggleActions";
	public const string PlayerSetup = "PlayerSetup";
	public const string NetworkMoveObject = "NetworkMoveObject";
	public const string NetworkRotateObject = "NetworkRotateObject";
}