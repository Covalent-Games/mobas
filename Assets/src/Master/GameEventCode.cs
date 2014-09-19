using UnityEngine;
using System.Collections;

public enum GameEventCode{

	PrimaryAction = 1
}

public class GameEventParameter{
	
	#region Common Parameters
	public const int ViewID = 1;
	public const int SenderViewID = 2;
	public const int Health = 3;
	#endregion
}