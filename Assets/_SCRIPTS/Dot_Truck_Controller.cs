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

    [Header("UI")]
    public Text speedCounter;
	public Slider speedBar;
    public Text lapCounter;
    bool finished = false;
    public int lap = 1;

    public float maxMotorTorque;
	public float maxSteeringAngle;
	public List<Dot_Truck> truck_Infos;
	public float motorSpeed;
	public float brakeForce = -1000000;
	public Rigidbody selfRigidbody;
	public Vector3 PreviousFramePosition;
	public Transform cam;
	public float Speed;
    bool canFinish = false;
    bool coroutineBool = true;
    bool resetCooldown = true;
    bool doOnce = true;
	int displayedSpeed;

	void Start()
	{
        selfRigidbody = this.gameObject.GetComponent<Rigidbody> ();
		if (isLocalPlayer) {
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = true;
            lap = 1;
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

        lapCounter.text = lap.ToString();
		speedBar.value = Speed;
		displayedSpeed = Mathf.RoundToInt (Speed);;
		speedCounter.text = displayedSpeed.ToString();

        if (lap > 3)
        {
            finished = true;
            lap = 3;
        }

        if (!canFinish && coroutineBool)
        {
            coroutineBool = false;
            StartCoroutine(CanFinish());
        }

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
            cam.transform.localPosition = new Vector3(-0.38f, 7.1f, -10.64f);
            cam.transform.localRotation = new Quaternion(0.174f, 0, 0, 0.985f);
        }

        if (Speed > 10)
        {
            selfRigidbody.AddForce(-transform.up * Time.deltaTime * 500000);
        }

        if (raceStarted && !finished)
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

            if (Input.GetKey(KeyCode.R) && resetCooldown)
            {
                selfRigidbody.velocity = Vector3.zero;
                transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                resetCooldown = false;
                StartCoroutine(ResetPos());
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

    IEnumerator CanFinish()
    {
        yield return new WaitForSeconds(5f);
        canFinish = true;
    }

    IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(10f);
        resetCooldown = true;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Lap") && canFinish)
        {
            lap++;
            coroutineBool = true;
            canFinish = false;
        }
        if (col.gameObject.CompareTag("Reset"))
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            selfRigidbody.velocity = Vector3.zero;
            selfRigidbody.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate () 
	{
		float movementPerFrame = Vector3.Distance (PreviousFramePosition, transform.position) ;
		Speed = movementPerFrame / Time.deltaTime;
		PreviousFramePosition = transform.position;
	}

}