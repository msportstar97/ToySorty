using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ColorTrigger : MonoBehaviour {

    public Canvas score;
    public string correct_color;
    static int toyPieceCount;
    GameLogicScript gameLogic;
    bool oneTrigger;

    // Use this for initialization
    void Start()
    {
        correct_color = this.tag;
        toyPieceCount = 4;
        gameLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<GameLogicScript>();
        oneTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        oneTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!oneTrigger)
        {
            if (other.GetComponent<LeftLegScript>() != null)
            {
                changeScore(other.GetComponent<LeftLegScript>().destroy());
                other.gameObject.SetActive(false);
                Destroy(other);
                oneTrigger = true;
            }
            else if (other.GetComponent<HeadScript>() != null)
            {
                changeScore(other.GetComponent<HeadScript>().destroy());
                other.gameObject.SetActive(false);
                Destroy(other);
                oneTrigger = true;
            }
            else if (other.GetComponent<BodyScript>() != null)
            {
                changeScore(other.GetComponent<BodyScript>().destroy());
                other.gameObject.SetActive(false);
                Destroy(other);
                oneTrigger = true;
            }
            else if (other.GetComponent<RightLegScript>() != null)
            {
                changeScore(other.GetComponent<RightLegScript>().destroy());
                other.gameObject.SetActive(false);
                Destroy(other);
                oneTrigger = true;
            }
        } 
    }

    private void changeScore(ArrayList colors)
    {
        int scoreChange = 0;
        for(int i = 0; i < colors.Count; i++)
        {
            if(colors[i].Equals(correct_color))
            {
                scoreChange++;
            }
            else
            {
                scoreChange--;
            }

            toyPieceCount--;
        }
        if(toyPieceCount <= 0)
        {
            toyPieceCount = 4;
            gameLogic.spawnToy();
        }
        score.GetComponent<ScoreScript>().incrementScore(scoreChange);
    }

    public void resetCount()
    {
        toyPieceCount = 4;
    }
}
