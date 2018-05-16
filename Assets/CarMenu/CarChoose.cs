using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;
<<<<<<< HEAD
=======
>>>>>>> parent of 946bed2... push
using UnityEngine.Networking;
=======
//using UnityEngine.Networking;
>>>>>>> parent of 3934c5b... trash

public class CarChoose : MonoBehaviour /*NetworkBehaviour*/ {

	public GameObject[] cars;
	public float rotationSpeed;
	public int whatCar;
	public int hidden;

	void Start () 
	{
		//whatCar++;
		for (int i = 0; i < cars.Length; i++)
		{
			cars [i].SetActive (false);
		}
		cars [0].SetActive (true);
	}

	void Update () 
	{
		transform.Rotate (0,rotationSpeed,0);

<<<<<<< HEAD
<<<<<<< HEAD
		carNumber = whatCar;

=======
>>>>>>> parent of 3934c5b... trash
		if(Input.GetKeyDown(KeyCode.W))
		{
			carNumber = whatCar;
			SceneManager.LoadScene (1);
		}

=======
>>>>>>> parent of 946bed2... push
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