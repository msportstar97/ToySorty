using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyFallScript : MonoBehaviour {

    public GameObject head, body, leftLeg, rightLeg, toy;
    GameObject[] parts;
    Vector3[] startPos;
	// Use this for initialization
	void Start () {
        startPos = new Vector3[4];
        parts = new GameObject[] { head, body, leftLeg, rightLeg };
        for (int i = 0; i < 4; i++)
        {
            startPos[i] = parts[i].transform.position - toy.transform.position;
        }
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), true);
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag.Equals("floor"))
        {
            resetPosition();
        }
    }

    public void resetPosition()
    {
        GameObject[] parts = new GameObject[] { head, body, leftLeg, rightLeg };
        for (int i = 0; i < 4; i++)
        {
            if(parts[i] != null)
            {
                if(parts[i].GetComponent<HeadScript>() != null)
                {
                    if(parts[i].GetComponent<HeadScript>().popped)
                    {
                        Destroy(parts[i].GetComponent<HeadScript>().popJoint);
                    }
                    if(!parts[i].GetComponent<HeadScript>().attached)
                    {
                        startPos[i] = new Vector3(.3f, 0f, -.12f);
                        parts[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                }
                else if(parts[i].GetComponent<BodyScript>() != null)
                {
                    if (parts[i].GetComponent<BodyScript>().popped)
                    {
                        Destroy(parts[i].GetComponent<BodyScript>().popJoint);
                    }
                    if(!parts[i].GetComponent<BodyScript>().attached)
                    {
                        startPos[i] = new Vector3(-.3f, 0f, 0f);
                        parts[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                }
                else if (parts[i].GetComponent<LeftLegScript>() != null)
                {
                    if (parts[i].GetComponent<LeftLegScript>().legJoint == null)
                    {
                        startPos[2] = new Vector3(-.1f, 0f, 0f);
                        startPos[3] = new Vector3(.1f, 0f, 0f);
                        parts[2].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                        parts[3].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                }
                parts[i].GetComponent<OVRGrabbable>().release();
                parts[i].transform.localPosition = startPos[i];
                parts[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                parts[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }  
        }
    }
}
