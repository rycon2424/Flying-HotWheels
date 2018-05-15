using UnityEngine;
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

	public float maxMotorTorque;
	public float maxSteeringAngle;
	public List<Dot_Truck> truck_Infos;
	public float motorSpeed;
	public float brakeForce = -1000000;
	public Rigidbody selfRigidbody;
	public Vector3 PreviousFramePosition;
	public float Speed;

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
			if (Speed > 0) {
				selfRigidbody.AddForce (transform.forward * Time.deltaTime * brakeForce);
				brakeTorque = maxMotorTorque;
				motorSpeed = 0;
			}
			if (Speed < 3 && motorSpeed < 1) {
				selfRigidbody.AddForce (transform.forward * Time.deltaTime * -500000);
				brakeTorque = maxMotorTorque;
				motorSpeed = 0;
			}
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