using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SyncedVars : NetworkBehaviour {

    public Text timer;
    public static bool gameStarted = false;
    public GameObject gate;
    public AudioSource track;
    bool playOnce = true;
    [SyncVar]
    public int countDown = 45;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        timer.text = countDown.ToString();
        if (gameStarted)
        {
            StartCoroutine(CountDown());
            gameStarted = false;
        }
        if (countDown == 16 && playOnce)
        {
            playOnce = false;
            track.Play();
        }
        if (countDown == 0)
        {
            Dot_Truck_Controller.raceStarted = true;
            Destroy(gate);
        }
	}

    IEnumerator CountDown()
    {
        for (int i = 0; i < 45; i++)
        {
            yield return new WaitForSeconds(1f);
            countDown = countDown - 1;
        }
    }
}
