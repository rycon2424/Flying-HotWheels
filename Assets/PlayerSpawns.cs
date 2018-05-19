using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerSpawns : NetworkManager {

	private GameObject[] Car = new GameObject[11];
	public static int playerCount;
	public int playersConnected;
	public static int countDown = 30;
	public Text countDownDisplay;
	public static bool startGame;
	public GameObject blockade;
	bool gameCanStart = false;
	bool once = true;

	void Update()
	{
		countDownDisplay.text = countDown.ToString ();
		playersConnected = playerCount;
		if (gameCanStart == true)
		{
			StartCoroutine(Seconds());
			gameCanStart = false;
		}

		if (countDown == 0)
		{
			Destroy(blockade);
		}
	}

	IEnumerator Seconds()
	{
		for (int i = 0; i < 30; i++)
		{
			yield return new WaitForSeconds (1);
			countDown = countDown - 1;
		}
	}


	public override void OnServerConnect(NetworkConnection conn)
	{
		
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (once == true)
		{
			gameCanStart = true;
			once = false;
		}
		playerCount++;
		if (playerCount == 1)
		{
			Car[0] = Instantiate(Resources.Load("Car1"), new Vector3(-5.73f, 0.4f, 64.33f), Quaternion.identity) as GameObject;
			Debug.Log("Player1");
			NetworkServer.AddPlayerForConnection(conn, Car[0], playerControllerId);
		}

		if (playerCount == 2)
		{
			Car[1] = Instantiate(Resources.Load("Car2"), new Vector3(5.68f, 0.4f, 50.44f), Quaternion.identity) as GameObject;
			Debug.Log("Player2");
			NetworkServer.AddPlayerForConnection(conn, Car[1], playerControllerId);
		}

		if (playerCount == 3)
		{
			Car[2] = Instantiate(Resources.Load("Car3"), new Vector3(-5.73f, 0.4f, 37.3f), Quaternion.identity) as GameObject;
			Debug.Log("Player3");
			NetworkServer.AddPlayerForConnection(conn, Car[2], playerControllerId);
		}

		if (playerCount == 4)
		{
			Car[3] = Instantiate(Resources.Load("Car4"), new Vector3(5.68f, 0.4f, 21.9f), Quaternion.identity) as GameObject;
			Debug.Log("Player4");
			NetworkServer.AddPlayerForConnection(conn, Car[3], playerControllerId);
		}

		if (playerCount == 5)
		{
			Car[4] = Instantiate(Resources.Load("Car5"), new Vector3(-6.1f, 0.4f, 7), Quaternion.identity) as GameObject;
			Debug.Log("Player5");
			NetworkServer.AddPlayerForConnection(conn, Car[4], playerControllerId);
		}

		if (playerCount == 6)
		{
			Car[5] = Instantiate(Resources.Load("Car6"), new Vector3(5.3f, 0.4f, -8.4f), Quaternion.identity) as GameObject;
			Debug.Log("Player6");
			NetworkServer.AddPlayerForConnection(conn, Car[5], playerControllerId);
		}

		if (playerCount == 7)
		{
			Car[6] = Instantiate(Resources.Load("Car3"), new Vector3(-6.1f ,0.4f, -23.2f), Quaternion.identity) as GameObject;
			Debug.Log("Player7");
			NetworkServer.AddPlayerForConnection(conn, Car[6], playerControllerId);
		}

		if (playerCount == 8)
		{
			Car[7] = Instantiate(Resources.Load("Car8"), new Vector3(5.3f, 0.4f, -37.2f), Quaternion.identity) as GameObject;
			Debug.Log("Player8");
			NetworkServer.AddPlayerForConnection(conn, Car[7], playerControllerId);
		}

		if (playerCount == 9)
		{
			Car[8] = Instantiate(Resources.Load("Car9"), new Vector3(-5.84f, 0.4f, -50.46f), Quaternion.identity) as GameObject;
			Debug.Log("Player9");
			NetworkServer.AddPlayerForConnection(conn, Car[8], playerControllerId);
		}

		if (playerCount == 10)
		{
			Car[9] = Instantiate(Resources.Load("Car10"), new Vector3(5.49f, 0.4f, -66.38f), Quaternion.identity) as GameObject;
			Debug.Log("Player10");
			NetworkServer.AddPlayerForConnection(conn, Car[9], playerControllerId);
		}

		if (playerCount == 11)
		{
			Car[10] = Instantiate(Resources.Load("Car11"), new Vector3(-6.29f, 0.4f, -80.88f), Quaternion.identity) as GameObject;
			Debug.Log("Player11");
			NetworkServer.AddPlayerForConnection(conn, Car[10], playerControllerId);
		}

		if (playerCount == 12)
		{
			Car[11] = Instantiate(Resources.Load("Car12"), new Vector3(5.3f, 0.4f, -96.5f), Quaternion.identity) as GameObject;
			Debug.Log("Player12");
			NetworkServer.AddPlayerForConnection(conn, Car[11], playerControllerId);
		}
	}
}