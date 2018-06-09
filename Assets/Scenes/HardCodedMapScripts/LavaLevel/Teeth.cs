using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Teeth : NetworkBehaviour {

    private float speed = 18;
    public float limitY;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.localPosition.y > limitY)
        {
            speed = -18;
        }
        else if (transform.localPosition.y < -limitY)
        {
            speed = 18;
        }
        
    }
    
}
