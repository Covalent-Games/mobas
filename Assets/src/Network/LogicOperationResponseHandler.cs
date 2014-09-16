//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using ExitGames.Client.Photon;


public class LogicOperationResponseHandler{
	
	/// <summary>
	/// Handles the operation response.
	/// </summary>
	/// <param name="response">Response from the server containing data from corresponding OperationRequest.</param>
	public static void HandleOperationResponse(OperationResponse response){
		
		switch ((LogicOperationCode)response.OperationCode){
			default:
				Debug.LogWarning(string.Format("OperationResponse unhandled: {0}", response.ToString()));
				break;

			case LogicOperationCode.GetSectorInfo:
				HandleGetSectorInfo(response);
				break;
			case LogicOperationCode.SpawnMasterClientProcess:
				HandleSpawnMasterClientProcess(response);
				break;				
		}
	}
	
	/// <summary>
	/// Handles the get sector info.
	/// </summary>
	/// <param name="response">Response.</param>
	static void HandleGetSectorInfo(OperationResponse response){
	
		//EXAMPLE
		//Debug.Log(response.Parameters[LogicParameterCode.SectorInfoDict]);
	}
	
	static void HandleSpawnMasterClientProcess(OperationResponse response){
	
		foreach (var d in response.Parameters){
			Debug.Log(d.Key);
			Debug.Log(d.Value);
		}
		
		PhotonNetwork.JoinRoom(response.Parameters[LogicParameterCode.RoomID]);
	}

}

