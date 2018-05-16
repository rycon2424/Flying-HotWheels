using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawns : NetworkManager {

	private GameObject[] Car = new GameObject[11];
	public static int playerCount;
	public int playersConnected;
	public int displayChosenCar;
	public static bool spawnMyCar = false;
	bool canSpawn = false;

	void Update()
	{
		playersConnected = playerCount;
		displayChosenCar = CarChoose.carNumber;
		if (spawnMyCar == true)
		{
			canSpawn = true;
			spawnMyCar = false;
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		playerCount++;
		if (playerCount == 1 && canSpawn == true)
		{
			if (CarChoose.carNumber == 0)
			{
				Car[0] = Instantiate(Resources.Load("Car1"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log("Player1");
				NetworkServer.AddPlayerForConnection(conn, Car[0], playerControllerId);
			}
			if (CarChoose.carNumber == 2)
			{
				Car[0] = Instantiate(Resources.Load("Car2"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log("Player1");
				NetworkServer.AddPlayerForConnection(conn, Car[0], playerControllerId);
			}
			if (CarChoose.carNumber == 3)
			{
				Car[0] = Instantiate(Resources.Load("Car3"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log("Player1");
				NetworkServer.AddPlayerForConnection(conn, Car[0], playerControllerId);
			}
		}

		if (playerCount == 2)
		{
			if (CarChoose.carNumber == 0)
			{
				Car[1] = Instantiate(Resources.Load("Car1"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log("Player2");
				NetworkServer.AddPlayerForConnection(conn, Car[1], playerControllerId);
			}
			if (CarChoose.carNumber == 2)
			{
				Car[1] = Instantiate(Resources.Load("Car2"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log("Player2");
				NetworkServer.AddPlayerForConnection(conn, Car[1], playerControllerId);
			}
			if (CarChoose.carNumber == 3)
			{
				Car[1] = Instantiate(Resources.Load("Car3"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log("Player2");
				NetworkServer.AddPlayerForConnection(conn, Car[1], playerControllerId);
			}

		}

		if (playerCount == 3)
		{
			Car[2] = Instantiate(Resources.Load("Car3"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			Debug.Log("Player3");
			NetworkServer.AddPlayerForConnection(conn, Car[2], playerControllerId);
		}
	}
}