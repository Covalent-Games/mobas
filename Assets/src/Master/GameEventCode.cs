using UnityEngine;
using System.Collections;

public enum GameEventCode{

	PrimaryAction = 1,
	SpawnPlayer,
}

//TODO: Urgent. Change these to Bytes. They do not have to be unique, and therefore can have more than 255. bytes = 2 bytes, int = 5 bytes.
public class GameEventParameter{
	
	#region Common Parameters | Must be unique
	public const int TargetViewID = 1;
	public const int SenderViewID = 2;
	public const int Health = 3;
	#endregion
	
	#region SpawnPlayer
	public const int CharacterName = 0;
	#endregion
}