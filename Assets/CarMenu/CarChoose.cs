using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CarChoose : NetworkBehaviour {

	public GameObject[] cars;
	public float rotationSpeed;
	public int whatCar;
	int hidden;

	public static int carNumber;

	void Start () 
	{
		for (int i = 0; i < cars.Length; i++)
		{
			cars [i].SetActive (false);
		}
		cars [0].SetActive (true);
	}

	void Update () 
	{
		transform.Rotate (0,rotationSpeed,0);

		carNumber = whatCar;

		if(Input.GetKeyDown(KeyCode.W))
		{
			PlayerSpawns.spawnMyCar = true;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			hidden = whatCar;
			if (whatCar == 0)
			{
				hidden++;
				cars [0].SetActive(false);
				whatCar++;
			}
			hidden = hidden - 1;
			cars [hidden].SetActive (false);
			if (whatCar == cars.Length)
			{
				hidden = 0;
				whatCar = 0;
				cars [whatCar].SetActive(true);
			}
			else 
			{
				cars [whatCar].SetActive(true);
				whatCar++;
			}
		}
	}
}