using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrilling : MonoBehaviour 

{
	
	public Vector3 PreviousFramePosition;
	public float Speed;
	public float shakeSpeed; 
	bool Up = true;
	bool Again = true;

	void Start () 
	{
		
	}

	void Update()
	{
		
		if (Speed < 40)
		{
			if(Again)
			{
				shakeSpeed = 0.05f;
				Again = false;
				StartCoroutine(Down());
			}
		}
		if (Speed > 40 && Speed < 60)
		{
			if(Again)
			{
				shakeSpeed = 0.03f;
				Again = false;
				StartCoroutine(Down());
			}
		}
		if (Speed > 60)
		{
			{
				shakeSpeed = 0.01f;
				Again = false;
				StartCoroutine(Down());
			}
		}
	}

	void FixedUpdate () 
	{
		float movementPerFrame = Vector3.Distance (PreviousFramePosition, transform.position) ;
		Speed = movementPerFrame / Time.deltaTime;
		PreviousFramePosition = transform.position;
	}

	IEnumerator Down()
	{
		yield return new WaitForSeconds (shakeSpeed);
		if(Up)
		{
			transform.Translate(0,0.05f,0);
			Up = false;
		}else
		{
			transform.Translate(0,-0.05f,0);
			Up = true;
		}
		Again = true;
	}
}
