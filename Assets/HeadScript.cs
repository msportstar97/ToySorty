using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScript : MonoBehaviour {

    public GameObject body, leftLeg, rightLeg;
    public HingeJoint joint;
    public FixedJoint popJoint;
    int rotations;
    bool fullRotation;
    public bool attached, popped;
    AudioSource popSound;

	// Use this for initialization
	void Start () {
        joint = gameObject.GetComponent<HingeJoint>();
        popJoint = null;
        rotations = 7;
        fullRotation = false;
        attached = true;
        popped = false;
        popSound = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if(attached)
        {
            if ((!fullRotation && joint.angle >= 80 && joint.angle <= 100) || (fullRotation && joint.angle <= -80 && joint.angle >= -100))
            {
                rotations--;
                fullRotation = !fullRotation;
            }
            if(rotations <= 0)
            {
                attached = false;
                popped = true;
                Destroy(joint);
                gameObject.transform.position += (body.transform.right * .01f);
                popJoint = gameObject.AddComponent<FixedJoint>();
                popJoint.connectedBody = body.GetComponent<Rigidbody>();
                gameObject.GetComponent<OVRGrabbable>().release();
                AudioSource audioSource = GetComponent<AudioSource>();
                popSound.Play();
            }
        }
        else if(popped)
        {
            if(gameObject.GetComponent<OVRGrabbable>().isGrabbed)
            {
                popped = false;
                Destroy(popJoint);
                body.GetComponent<BodyScript>().setLimits();
            }
        }
	}

    public ArrayList destroy()
    {
        ArrayList colors = new ArrayList();
        colors.Add(gameObject.tag);
        if (body != null)
        {
            if(this.joint != null || this.popJoint != null)
            {
                
                if(leftLeg != null)
                {
                    if(body.GetComponent<BodyScript>().joint != null || body.GetComponent<BodyScript>().popJoint != null)
                    {
                        
                        if(rightLeg != null)
                        {
                            if(leftLeg.GetComponent<LeftLegScript>().legJoint != null)
                            {
                                colors.Add(rightLeg.tag);
                                rightLeg.SetActive(false);
                                Destroy(rightLeg);
                            }
                        }
                        colors.Add(leftLeg.tag);
                        leftLeg.SetActive(false);
                        Destroy(leftLeg);
                    }
                }
                colors.Add(body.tag);
                body.SetActive(false);
                Destroy(body);
            }
            
        }
        return colors;
    }
}
