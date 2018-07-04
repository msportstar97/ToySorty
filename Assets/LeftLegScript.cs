using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLegScript : MonoBehaviour {

    public GameObject leftLeg, rightLeg, body, head;
    public FixedJoint legJoint;
    HingeJoint bodyJoint;
    AudioSource popSound;


	// Use this for initialization
	void Start () {
        legJoint = leftLeg.GetComponent<FixedJoint>();
        bodyJoint = body.GetComponent<HingeJoint>();
        popSound = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (leftLeg.GetComponent<OVRGrabbable>().isGrabbed && rightLeg.GetComponent<OVRGrabbable>().isGrabbed && legJoint != null && bodyJoint == null)
        {
            Destroy(legJoint);
            popSound.Play();
        }
	}

    public ArrayList destroy()
    {
        ArrayList colors = new ArrayList();
        colors.Add(gameObject.tag);
        if (body != null)
        {
            if (body.GetComponent<BodyScript>().joint != null || body.GetComponent<BodyScript>().popJoint != null)
            {
                
                if (head != null)
                {
                    if (head.GetComponent<HeadScript>().joint != null || head.GetComponent<HeadScript>().popJoint != null)
                    {
                        colors.Add(head.tag);
                        head.SetActive(false);
                        Destroy(head);
                    }
                }
                colors.Add(body.tag);
                body.SetActive(false);
                Destroy(body);
            }
        }
        if (rightLeg != null && legJoint != null)
        {
            colors.Add(rightLeg.tag);
            rightLeg.SetActive(false);
            Destroy(rightLeg);
        }
        return colors;
    }
}
