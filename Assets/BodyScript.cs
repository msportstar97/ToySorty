using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyScript : MonoBehaviour {

    public GameObject leftLeg, rightLeg, head;
    public HingeJoint joint;
    public FixedJoint popJoint;
    int shakeCount;
    bool backShake;
    public bool popped, attached;
    AudioSource popSound;

	// Use this for initialization
	void Start () {
        joint = gameObject.GetComponent<HingeJoint>();
        popJoint = null;
        shakeCount = 7;
        backShake = true;
        attached = true;
        JointLimits limits = joint.limits;
        limits.min = 0;
        limits.max = 0;
        joint.limits = limits;
        popSound = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if(attached) //&& head is not attached
        {
            if (head == null)
            {
                if ((backShake && joint.angle <= -52) || (!backShake && joint.angle >= 52))
                {
                    shakeCount--;
                    backShake = !backShake;
                }
                if (shakeCount <= 0)
                {
                    attached = false;
                    popped = true;
                    Destroy(joint);
                    popJoint = gameObject.AddComponent<FixedJoint>();
                    popJoint.connectedBody = leftLeg.GetComponent<Rigidbody>();
                    popSound.Play();

                }
            }
            else if(head.GetComponent<HingeJoint>() == null)
            {
                if ((backShake && joint.angle <= -52) || (!backShake && joint.angle >= 52))
                {
                    shakeCount--;
                    backShake = !backShake;
                }
                if (shakeCount <= 0)
                {
                    attached = false;
                    popped = true;
                    Destroy(joint);
                    gameObject.transform.position += (leftLeg.transform.forward * .02f);
                    popJoint = gameObject.AddComponent<FixedJoint>();
                    popJoint.connectedBody = leftLeg.GetComponent<Rigidbody>();
                    popSound.Play();
                }
            }
        }
        else if(popped)
        {
            if(gameObject.GetComponent<OVRGrabbable>().isGrabbed)
            {
                popped = false;
                Destroy(popJoint);
            }
        }
	}

    public void setLimits()
    {
        if(joint != null)
        {
            JointLimits limits = joint.limits;
            limits.min = -55;
            limits.max = 55;
            joint.limits = limits;
        }
    }

    public ArrayList destroy()
    {
        ArrayList colors = new ArrayList();
        colors.Add(gameObject.tag);
        if (head != null)
        {
            if(head.GetComponent<HeadScript>().joint != null || head.GetComponent<HeadScript>().popJoint != null)
            {
                colors.Add(head.tag);
                head.SetActive(false);
                Destroy(head);
            }
        }
        if (leftLeg != null)
        {
            if(this.joint != null || this.popJoint != null)
            {
                
                if (rightLeg != null)
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
        return colors;
    }
}
