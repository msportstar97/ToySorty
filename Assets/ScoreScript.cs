using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreScript : MonoBehaviour {

    public Text score;
    public int points;
    public Text level;
    public int lev;
    public Text livesText;
    public int lives;
    public GameObject end;
    public GameObject start;
    public GameObject won;
    public Text doubleText;
    public float timer;
    public int minutes;
    public int seconds;
    public Text timerText;
    GameLogicScript gameLogic;
    public bool playing, doublePoints, paused, lost;
    float doublePointsTimer;
    AudioSource powerupSound;
    //public GameObject levelCard;
    //int goal;

	// Use this for initialization
	void Start () {
        points = 0;
        score.text = "Score: " + points;
        lev = 1;
        level.text = "Level " + lev;
        lives = 3;
        livesText.text = "Lives: " + lives;
        start.SetActive(true);
        end.SetActive(false);
        won.SetActive(false);
        timer = 30;
        minutes = Mathf.FloorToInt(timer / 60F);
        seconds = Mathf.FloorToInt(timer%60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        gameLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<GameLogicScript>();
        playing = false;
        doublePoints = false;
        paused = false;
        lost = false;
        doublePointsTimer = 0f;
        powerupSound = gameObject.GetComponent<AudioSource>();
       // goal = 20;
    }
	    
	// Update is called once per frame
	void Update () {
        if(playing)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            minutes = Mathf.FloorToInt(timer / 60F);
            seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            if (timer <= 0 /*|| points >= goal*/)
            {
                incrementLevel();
            }
            if (timer <= 10)
            {
                timerText.color = Color.red;
            }
            if (doublePoints)
            {
                doublePointsTimer -= Time.deltaTime;
                int doubleSeconds = Mathf.FloorToInt(doublePointsTimer % 60) + 1;
                doubleText.text = "Double Points!\n" + doubleSeconds;
                if (doublePointsTimer <= 0)
                {
                    doublePoints = false;
                    doubleText.text = "";
                }
            }
        }
        if (!playing && paused && Input.GetButton("Button A"))
        {
            paused = false;
            playing = true;
            gameLogic.startLevel();
        }

    }

    public void startGame()
    {
        start.SetActive(false);
        playing = true;
        points = 0;
    }

    public void incrementLevel()
    {
        if (lev == 1)
        {
            if (points < 16)
            {
                endGame();
            }
            else
            {
                timer = 50;
            }
        } else if (lev == 2)
        {
            if (points < 16)
            {
                endGame();
            } else
            {
                timer = 75;
            }
        } else if (lev == 3)
        {
            if (points < 16)
            {
                endGame();
            } else
            {
                timer = 90;
            }
        }
        else if (lev == 4)
        {
            if (points < 16)
            {
                endGame();
            } else
            {
                wonGame();
            }
        }
        if(lev != 4 && !lost)
        {
            doublePoints = false;
            doublePointsTimer = 0f;
            doubleText.text = "";
            lev += 1;
            points = 0;
            score.text = "Score: " + points;
            gameLogic.incrementLevel();
            timerText.color = Color.white;
            level.text = "Level " + lev;
            minutes = Mathf.FloorToInt(timer / 60F);
            seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            playing = false;
            paused = true;
        }

    }

    public void incrementScore(int scoreChange)
    {
        if (playing)
        {
            if (doublePoints)
            {
                points += 2 * scoreChange;
            }
            else
            {
                points += scoreChange;
            }
            score.text = "Score: " + points;
        }
    }

    public void decrementScore()
    {
        if(playing)
        {
            points--;
            score.text = "Score: " + points;
        }
    }

    public void loseLife()
    {
        lives--;
        if(lives == 0)
        {
            endGame();  
        }
        livesText.text = "Lives: " + lives;
    }

    public void addLife()
    {
        lives++;
        livesText.text = "Lives: " + lives;
    }

    public void addTime()
    {
        timer += 5;
        timerText.color = Color.white;
    }

    public void doublePointsMode()
    {
        doublePoints = true;
        doublePointsTimer = 8f;
    }

    public bool getDoublePoints()
    {
        return doublePoints;
    }

    public void endGame()
    {
        end.SetActive(true);
        playing = false;
        lost = true;
        gameLogic.endGame();
    }

    public void wonGame()
    {
        won.SetActive(true);
        playing = false;
        gameLogic.endGame();
    }

    public void playPowerupSound()
    {
        powerupSound.Play();
    }
}
