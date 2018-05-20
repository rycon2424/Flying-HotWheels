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
    bool doOnce = true;
	int displayedSpeed;

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

        if (transform.localEulerAngles.z > 70 && transform.localEulerAngles.z < 290 && doOnce)
        {
            StartCoroutine(ResetCheck());
            doOnce = false;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            cam.transform.localPosition = new Vector3(0, 5.5f, 10);
            cam.transform.localRotation = new Quaternion(0, -0.985f, 0.174f, 0);
        }
        else
        {
            cam.transform.localPosition = new Vector3(-0.2675995f, 7.1f, -9.33f);
            cam.transform.localRotation = new Quaternion(0.259f, 0, 0, 0.966f);
        }

        if (Speed > 10)
        {
            selfRigidbody.AddForce(-transform.up * Time.deltaTime * 500000);
        }

        if (raceStarted)
        {
           
            motorSpeed = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));

            if (brakeTorque > 0.001)
            {
                brakeTorque = maxMotorTorque;
                motorSpeed = 0;
            }
            else
            {
                brakeTorque = 0;
            }
            
            foreach (Dot_Truck truck_Info in truck_Infos)
            {
                if (truck_Info.steering == true)
                {
                    truck_Info.leftWheel.steerAngle = truck_Info.rightWheel.steerAngle = ((truck_Info.reverseTurn) ? -1 : 1) * steering;
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

            if (Input.GetKey(KeyCode.S) && Speed < 10)
            {
                selfRigidbody.AddForce(transform.forward * Time.deltaTime * -700000);
                motorSpeed = 0;
                if (Speed > 10)
                {
                    selfRigidbody.AddForce(transform.forward * Time.deltaTime * 700000);
                }
            }

            if (Input.GetKey(KeyCode.W))
            {
                return;
            }
            else if (Speed > 0 && !Input.GetKey(KeyCode.S))
            {
                motorSpeed = 0;
                if (Speed > 30)
                {
                    selfRigidbody.AddForce(transform.forward * Time.deltaTime * -1000000);
                }
                else if (Speed > 5)
                {
                    selfRigidbody.AddForce(transform.forward * Time.deltaTime * -700000);
                }
                else if (Speed > 1)
                {
                    selfRigidbody.AddForce(transform.forward * Time.deltaTime * -50000);
                }
                else if (Speed > 0)
                {
                    selfRigidbody.AddForce(transform.forward * Time.deltaTime * -10000);
                }
            }
        }

	}

    IEnumerator ResetCheck()
    {
        
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);
            if (transform.localEulerAngles.z > 70 && transform.localEulerAngles.z < 290)
            {
                Debug.Log(i);
                if (i == 4)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    doOnce = true;
                }
            }
            else
            {
                StopCoroutine(ResetCheck());
                doOnce = true;
            }
        }
    }

    void FixedUpdate () 
	{
		float movementPerFrame = Vector3.Distance (PreviousFramePosition, transform.position) ;
		Speed = movementPerFrame / Time.deltaTime;
		PreviousFramePosition = transform.position;
	}

}