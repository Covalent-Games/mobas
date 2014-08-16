
using UnityEngine;
using System;

/// <summary>
/// Holds all request codes for custom game logic.
/// Examples: Get status of persistent map, Calculate damage of shot, or use a skill.
/// </summary>
public class LogicOperationCode{
	
	/// <summary>
	/// Key for parameter dictionary who's value is the operation to run on the server.
	/// </summary>
	public const int LogicRequestID = 0;
	
	public const int GetSectorInfo = 1;
		
}


