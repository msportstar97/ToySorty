using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLegScript : MonoBehaviour {

    // Use this for initialization
    public GameObject leftLeg, body, head;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ArrayList destroy()
    {
        ArrayList colors = new ArrayList();
        colors.Add(gameObject.tag);
        if(leftLeg != null)
        {
            if(leftLeg.GetComponent<LeftLegScript>().legJoint != null)
            {
                
                if (body != null)
                {
                    if(body.GetComponent<BodyScript>().joint != null || body.GetComponent<BodyScript>().popJoint != null)
                    {
                        
                        if (head != null)
                        {
                            if(head.GetComponent<HeadScript>().joint != null || head.GetComponent<HeadScript>().popJoint != null)
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
                colors.Add(leftLeg.tag);
                leftLeg.SetActive(false);
                Destroy(leftLeg);
            }     
        }
        
        return colors;
    }
}
