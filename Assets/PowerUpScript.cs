using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    string[] types = { "Life", "Points", "Time" };
    string powerUpType;
    public Material red, yellow, blue;
    ScoreScript scoreScript;
    float despawnTimer;
    public bool copy = false;
    public bool tutorial = false;
    public bool active;
	// Use this for initialization
	void Start () {
        System.Random random = new System.Random();
        double odds = random.NextDouble();
        int choice = 2;
        if(odds < .15)
        {
            choice = 0;
        }
        else if(odds < .6)
        {
            choice = 1;
        }
        powerUpType = types[choice];
        despawnTimer = 5.0f;
        active = false;
        scoreScript = GameObject.FindGameObjectWithTag("scoreboard").GetComponent<ScoreScript>();
        if (copy)
        {
            drop();
        }
        setColor();
	}
	
	// Update is called once per frame
	void Update () {
        if(active)
        {
            despawnTimer -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - .001f, transform.position.z);
            if (despawnTimer <= 0)
            {
                Destroy(this.gameObject);
            }
            if (this.gameObject.GetComponent<OVRGrabbable>().isGrabbed)
            {
                activatePowerUp();
            }
        }
	}

    void drop()
    {
        active = true;
    }

    void activatePowerUp()
    {
        if(!tutorial)
        {    
            if (powerUpType.Equals("Life"))
            {
                scoreScript.addLife();
            }
            else if (powerUpType.Equals("Points"))
            {
                scoreScript.doublePointsMode();
            }
            else
            {
                scoreScript.addTime();
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<GameLogicScript>().tutorialPowerupGrabbed = true;
        }
        scoreScript.playPowerupSound();
        Destroy(this.gameObject);
    }

    void setColor()
    {
        if(powerUpType.Equals("Life"))
        {
            gameObject.GetComponent<Renderer>().material = red;
        }
        else if(powerUpType.Equals("Points"))
        {
            gameObject.GetComponent<Renderer>().material = yellow;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = blue;
        }
    }

}
