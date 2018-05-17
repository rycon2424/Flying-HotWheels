using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dot_Truck : System.Object
{
	public WheelCollider leftWheel;
	public GameObject leftWheelMesh;
	public WheelCollider rightWheel;
	public GameObject rightWheelMesh;
	public bool motor;
	public bool steering;
	public bool reverseTurn;
}

public class Dot_Truck_Controller : NetworkBehaviour {

	public static bool raceStarted = false;

	public Text speedCounter;
	public Slider speedBar;

	public float maxMotorTorque;
	public float maxSteeringAngle;
	public List<Dot_Truck> truck_Infos;
	public float motorSpeed;
	public float brakeForce = -1000000;
	public Rigidbody selfRigidbody;
	public Vector3 PreviousFramePosition;
	public Transform cam;
	public float Speed;
	int displayedSpeed;
//	public float rotationSpeed;

	void Start()
	{
		selfRigidbody = this.gameObject.GetComponent<Rigidbody> ();
		if (isLocalPlayer) {
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = true;
		} else {
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = false;
		}
	}

	public void VisualizeWheel(Dot_Truck wheelPair)
	{
		Quaternion rot;
		Vector3 pos;
		wheelPair.leftWheel.GetWorldPose ( out pos, out rot);
		wheelPair.leftWheelMesh.transform.position = pos;
		wheelPair.leftWheelMesh.transform.rotation = rot;
		wheelPair.rightWheel.GetWorldPose ( out pos, out rot);
		wheelPair.rightWheelMesh.transform.position = pos;
		wheelPair.rightWheelMesh.transform.rotation = rot;
	}

	public void Update()
	{
		if (!isLocalPlayer) 
		{
			return;
		}

		speedBar.value = Speed;
		displayedSpeed = Mathf.RoundToInt (Speed);;
		speedCounter.text = displayedSpeed.ToString();

		/*if (transform.localEulerAngles.z > 15 && transform.localEulerAngles.z < 180)
		{
			Debug.Log ("LMAO");
			transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
		}

		if (transform.localEulerAngles.z < 345 && transform.localEulerAngles.z > 180)
		{
			Debug.Log ("LMAO");
			transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
		}*/

		if (Input.GetKey (KeyCode.Tab)) 
		{
			cam.transform.localPosition = new Vector3 (0, 5.5f, 10);
			cam.transform.localRotation = new Quaternion (0, -0.985f, 0.174f, 0);
		} 
		else
		{
			cam.transform.localPosition = new Vector3 (-0.2675995f, 7.1f, -9.33f);
			cam.transform.localRotation = new Quaternion (0.259f, 0, 0, 0.966f);
		}
		
		motorSpeed = maxMotorTorque * Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));
		if (brakeTorque > 0.001) {
			brakeTorque = maxMotorTorque;
			motorSpeed = 0;
		} 
		else
		{
			brakeTorque = 0;
		}

		if (Input.GetKey(KeyCode.S))
		{
			if (Speed > 1) {
				brakeTorque = maxMotorTorque;
				motorSpeed = 0;
			}
			selfRigidbody.AddForce (transform.forward * Time.deltaTime * -500000);
		}

		foreach (Dot_Truck truck_Info in truck_Infos)
		{
			if (truck_Info.steering == true) {
				truck_Info.leftWheel.steerAngle = truck_Info.rightWheel.steerAngle = ((truck_Info.reverseTurn)?-1:1)*steering;
			}

			if (truck_Info.motor == true)
			{
				truck_Info.leftWheel.motorTorque = motorSpeed;
				truck_Info.rightWheel.motorTorque = motorSpeed;
			}

			truck_Info.leftWheel.brakeTorque = brakeTorque;
			truck_Info.rightWheel.brakeTorque = brakeTorque;

			VisualizeWheel(truck_Info);
		}

	}

	void FixedUpdate () 
	{
		float movementPerFrame = Vector3.Distance (PreviousFramePosition, transform.position) ;
		Speed = movementPerFrame / Time.deltaTime;
		PreviousFramePosition = transform.position;
	}

}