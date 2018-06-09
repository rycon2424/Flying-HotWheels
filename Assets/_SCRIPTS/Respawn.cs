using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public Vector3 respawnLoc;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Car")
        {
            Debug.Log("collide?");
            //col.transform.position = new Vector3(respawnLoc.x, respawnLoc.y, respawnLoc.z);
            col.transform.position = respawnLoc;
        }
    }

}
