using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    float timer, despawnTimer;
    public bool active, despawn, exploded;
    public GameObject scoreboard;
    Vector3 bombOrigin;
    public bool copy = false;
    public bool tutorial = false;
    ScoreScript scoreScript;
    public AudioSource bombFuse, bombExplode;
    ParticleSystem explosion;

    // Use this for initialization
    void Start () {
        timer = 6.0f;
        despawnTimer = 3.0f;
        active = false;
        despawn = false;
        exploded = false;
        scoreboard = GameObject.FindGameObjectWithTag("scoreboard");
        scoreScript = scoreboard.GetComponent<ScoreScript>();
        bombFuse = gameObject.GetComponents<AudioSource>()[0];
        bombExplode = gameObject.GetComponents<AudioSource>()[1];
        bombOrigin = gameObject.transform.position;
        explosion = gameObject.GetComponent<ParticleSystem>();
        explosion.Pause();
        if (copy)
        {
            activate();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(active)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                loseLife();
            }
            if (Vector3.Distance(bombOrigin, transform.position) > 2) 
            {
                deactivate();
            }
        }
        else if(despawn)
        {
            if(despawnTimer > 0)
            {
                despawnTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
		
	}

    void loseLife()
    {
        if(!tutorial)
        {
            scoreScript.loseLife();
        }
        exploded = true;
        explosion.Play();
        bombExplode.Play();
        active = false;
        despawn = true;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    public void activate()
    {
        active = true;
        bombFuse.Play();
    }

    void deactivate()
    {
        bombFuse.Stop();
        active = false;
        despawn = true;
    }
}
