using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawns : NetworkManager {

	private GameObject[] Car = new GameObject[11];
	public static int playerCount;
	public int playersConnected;

	void Update()
	{
		playersConnected = playerCount;
	}


	public override void OnServerConnect(NetworkConnection conn)
	{
		
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		playerCount++;
		if (playerCount == 1)
		{
			Car[0] = Instantiate(Resources.Load("Car5"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			Debug.Log("Player1");
			NetworkServer.AddPlayerForConnection(conn, Car[0], playerControllerId);
		}

		if (playerCount == 2)
		{
			Car[1] = Instantiate(Resources.Load("Car2"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			Debug.Log("Player2");
			NetworkServer.AddPlayerForConnection(conn, Car[1], playerControllerId);
		}

		if (playerCount == 3)
		{
			Car[2] = Instantiate(Resources.Load("Car3"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			Debug.Log("Player3");
			NetworkServer.AddPlayerForConnection(conn, Car[2], playerControllerId);
		}
	}
}