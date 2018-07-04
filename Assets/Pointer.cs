using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

    // Use this for initialization
    RaycastHit raycast;
    float timer;
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if ((Input.GetButton("Button A")))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycast, Mathf.Infinity))
            {
                ToyFallScript tfs = raycast.transform.gameObject.GetComponent<ToyFallScript>();
                if (tfs != null)
                {
                    tfs.resetPosition();
                    timer = 1f;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<GameLogicScript>().tutorialPointer = true;
                }
            }
        }

    }
}
