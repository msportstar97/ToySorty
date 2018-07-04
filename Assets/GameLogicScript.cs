using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour {

    float bombTimer, powerUpTimer;
    public GameObject bomb, powerUp, toy, leftLeg, startGameBlock, tutorialBlock, beginText, tutText1, tutText2, tutText3, tutText4, tutText5, tutText6, tutText7, tutVid1, tutVid2, tutVid3, tutVid4, tutVid5, logo, level2, level3, level4, redTrigger, menu;
    GameObject toyClone, powerUpClone, bombClone;
    public Material red, blue, green, yellow, purple;
    public bool tutorialPowerupGrabbed, tutorialPointer;
    bool playing, tutorial, gameOver, pressed, start, paused, pauseMen;
    int tutorialStep;
    double xMax = .543;
    double xMin = -.543;
    double zMax = -1.808;
    double zMin = -2.111;
    Vector3[] bombLocations = { new Vector3(.645f, 1.355f, -1.6531f),
    new Vector3( .869f, 1.355f, -1.6531f),
    new Vector3( -.2771f, 1.355f, -1.9628f),
    new Vector3( .237f, 1.355f, -1.9628f),
    new Vector3( -.6789f, 1.355f, -1.6387f),
    new Vector3( -.833f, 1.355f, -1.095f),
    new Vector3( -.4176f, 1.355f, -1.4074f),
    new Vector3( .53f, 1.355f, -1.4074f), };
    System.Random random;
    ScoreScript scoreScript;
    public int level;
    float delay;
    BombScript bombScript;
    PowerUpScript powerUpScript;

    // Use this for initialization
    void Start () {
        random = new System.Random();
        bombTimer = 10.0f;
        powerUpTimer = 20.0f;
        delay = 0f;
        playing = false;
        tutorial = false;
        gameOver = false;
        start = false;
        pressed = false;
        paused = false;
        pauseMen = false;
        tutorialPowerupGrabbed = false;
        tutorialPointer = false;
        tutText1.SetActive(false);
        tutText2.SetActive(false);
        tutText3.SetActive(false);
        tutText4.SetActive(false);
        tutText5.SetActive(false);
        tutText6.SetActive(false);
        tutText7.SetActive(false);
        tutVid1.SetActive(false);
        tutVid2.SetActive(false);
        tutVid3.SetActive(false);
        tutVid4.SetActive(false);
        tutVid5.SetActive(false);
        level2.SetActive(false);
        level3.SetActive(false);
        level4.SetActive(false);
        menu.SetActive(false);
        scoreScript = GameObject.FindGameObjectWithTag("scoreboard").GetComponent<ScoreScript>();
        level = scoreScript.lev;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Start"))
        {
            menu.SetActive(true);
            pauseMen = true;
            if (bombClone != null)
            {
                bombScript = bombClone.GetComponent<BombScript>();
                bombScript.bombFuse.Pause();
            }
            if (powerUpClone != null)
            {
                powerUpScript = powerUpClone.GetComponent<PowerUpScript>();
                powerUpScript.active = false;
            }
            Time.timeScale = 0;
        }

        if (pauseMen)
        {
            if (Input.GetButton("Button A"))
            {
                menu.SetActive(false);
                pauseMen = false;
                Time.timeScale = 1;
                if (bombClone != null)
                {
                    bombScript = bombClone.GetComponent<BombScript>();
                    bombScript.bombFuse.UnPause();
                }
                if (powerUpClone != null)
                {
                    powerUpScript = powerUpClone.GetComponent<PowerUpScript>();
                    powerUpScript.active = true;
                }
            } else if (Input.GetButton("Button X"))
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
				    Application.Quit();
                #endif
            }
        }

        if (playing)
        {
            bombTimer -= Time.deltaTime;
            powerUpTimer -= Time.deltaTime;
            if (bombTimer <= 0)
            {
                spawnBomb();
            }
            if (powerUpTimer <= 0)
            {
                spawnPowerUp();
            }
            level = scoreScript.lev;
        }
        else if (tutorial)
        {
            if (tutorialStep == 1 && toyClone.transform.GetChild(3).GetComponent<HeadScript>().joint == null)
            {
                tutText1.SetActive(false);
                tutText2.SetActive(true);
                tutVid1.SetActive(false);
                tutVid2.SetActive(true);
                tutorialStep++;
            }
            else if (tutorialStep == 2 && toyClone.transform.GetChild(2).GetComponent<BodyScript>().joint == null)
            {
                tutText2.SetActive(false);
                tutText3.SetActive(true);
                tutVid2.SetActive(false);
                tutVid3.SetActive(true);
                tutorialStep++;
            }
            else if (tutorialStep == 3 && toyClone.transform.GetChild(0).GetComponent<LeftLegScript>().legJoint == null)
            {
                tutText3.SetActive(false);
                tutText4.SetActive(true);
                tutVid3.SetActive(false);
                tutVid4.SetActive(true);
                tutorialStep++;
                spawnTutorialBomb();
            }
            else if (tutorialStep == 4 && bombClone.GetComponent<BombScript>().exploded)
            {
                spawnTutorialBomb();
            }
            else if (tutorialStep == 4 && !bombClone.GetComponent<BombScript>().active)
            {
                tutText4.SetActive(false);
                tutText5.SetActive(true);
                tutVid4.SetActive(false);
                tutVid5.SetActive(true);
                tutorialStep++;
                spawnTutorialPowerUp();
            }
            else if (tutorialStep == 5 && tutorialPowerupGrabbed)
            {
                tutText5.SetActive(false);
                tutText6.SetActive(true);
                tutVid5.SetActive(false);
                tutorialStep++;
                if(toyClone != null)
                {
                    Destroy(toyClone);
                }
                spawnToy();
            }
            else if (tutorialStep == 5 && powerUpClone == null)
            {
                spawnTutorialPowerUp();
            }
            else if (tutorialStep == 6 && tutorialPointer)
            {
                tutText6.SetActive(false);
                tutText7.SetActive(true);
                tutorialStep++;
                delay = 1f;
            }
            else if (tutorialStep == 7 && Input.GetButton("Button A") && delay <= 0)
            {
                tutText7.SetActive(false);
                startGame();
            }
            else if(delay > 0)
            {
                delay -= Time.deltaTime;
            }
        }
        else if (!gameOver && !paused)
        {
            if (startGameBlock.GetComponent<OVRGrabbable>().isGrabbed)
            {
                tutText7.SetActive(true);
                start = true;
                Destroy(startGameBlock);
                Destroy(tutorialBlock);
                beginText.SetActive(false);
                logo.SetActive(false);
                paused = true;
            }
            else if (tutorialBlock.GetComponent<OVRGrabbable>().isGrabbed)
            {
                startTutorial();
                Destroy(startGameBlock);
                Destroy(tutorialBlock);
                beginText.SetActive(false);
                logo.SetActive(false);
                paused = true;
            }
        }
        else if (paused)
        {
            if (start && Input.GetButton("Button A"))
            {
                tutText7.SetActive(false);
                startGame();
                paused = false;
            }
        }
    }

    public void startGame()
    {
        if(toyClone != null)
        {
            Destroy(toyClone);
        }
        playing = true;
        scoreScript.startGame();
        spawnToy();
    }

    public void startTutorial()
    {
        tutorialStep = 1;
        tutorial = true;
        tutText1.SetActive(true);
        tutVid1.SetActive(true);
        spawnToy();
    }

    void spawnBomb()
    {
        bombTimer = random.Next(5) + 12;
        int bombChoose = random.Next(bombLocations.Length);
        bombClone = Object.Instantiate(bomb, bombLocations[bombChoose], new Quaternion(0f, 0f, 0f, 0f));
        bombClone.GetComponent<BombScript>().copy = true;
    }

    void spawnTutorialBomb()
    {
        int bombChoose = random.Next(bombLocations.Length);
        bombClone = Object.Instantiate(bomb, bombLocations[bombChoose], new Quaternion(0f, 0f, 0f, 0f));
        bombClone.GetComponent<BombScript>().copy = true;
        bombClone.GetComponent<BombScript>().tutorial = true;
    }

    void spawnPowerUp()
    {
        powerUpTimer = random.Next(6) + 17;
        float x = (float)(random.NextDouble() * (xMax - xMin) + xMin);
        float z = (float)(random.NextDouble() * (zMax - zMin) + zMin);
        powerUpClone = Object.Instantiate(powerUp, new Vector3(x, 1.709f, z), new Quaternion(0f, 0f, 0f, 0f));
        powerUpClone.transform.Rotate(new Vector3(90, 0, 0));
        powerUpClone.GetComponent<PowerUpScript>().copy = true;
    }

    void spawnTutorialPowerUp()
    {
        float x = (float)(random.NextDouble() * (xMax - xMin) + xMin);
        float z = (float)(random.NextDouble() * (zMax - zMin) + zMin);
        powerUpClone = Object.Instantiate(powerUp, new Vector3(x, 1.709f, z), new Quaternion(0f, 0f, 0f, 0f));
        powerUpClone.transform.Rotate(new Vector3(90, 0, 0));
        powerUpClone.GetComponent<PowerUpScript>().copy = true;
        powerUpClone.GetComponent<PowerUpScript>().tutorial = true;
    }

    public void spawnToy()
    {

        ArrayList colors = new ArrayList { "red", "blue", "yellow", "green", "purple" };
        toyClone = Object.Instantiate(toy, new Vector3(.034f, 1.503f, -1.7065f), new Quaternion(0f, 0f, 0f, 0f));
        LeftLegScript toyScript = toyClone.transform.GetChild(0).GetComponent<LeftLegScript>();
        GameObject head = toyScript.head;
        GameObject body = toyScript.body;
        GameObject leftLeg = toyScript.leftLeg;
        GameObject rightLeg = toyScript.rightLeg;
        int chooseColor = random.Next(colors.Count);
        if (level == 1)
        {
            head.tag = "" + colors[chooseColor];
            body.tag = "" + colors[chooseColor];
            leftLeg.tag = "" + colors[chooseColor];
            rightLeg.tag = "" + colors[chooseColor];
        }
        else if (level == 2)
        {
            head.tag = "" + colors[chooseColor];
            colors.RemoveAt(chooseColor);
            chooseColor = random.Next(colors.Count);
            body.tag = "" + colors[chooseColor];
            leftLeg.tag = "" + colors[chooseColor];
            rightLeg.tag = "" + colors[chooseColor];
        }
        else if (level == 3)
        {
            head.tag = "" + colors[chooseColor];
            colors.RemoveAt(chooseColor);
            chooseColor = random.Next(colors.Count);
            body.tag = "" + colors[chooseColor];
            colors.RemoveAt(chooseColor);
            chooseColor = random.Next(colors.Count);
            leftLeg.tag = "" + colors[chooseColor];
            rightLeg.tag = "" + colors[chooseColor];
        }
        else
        {
            head.tag = "" + colors[chooseColor];
            colors.RemoveAt(chooseColor);
            chooseColor = random.Next(colors.Count);
            body.tag = "" + colors[chooseColor];
            colors.RemoveAt(chooseColor);
            chooseColor = random.Next(colors.Count);
            leftLeg.tag = "" + colors[chooseColor];
            colors.RemoveAt(chooseColor);
            chooseColor = random.Next(colors.Count);
            rightLeg.tag = "" + colors[chooseColor];
        }

        head.transform.GetChild(0).GetComponent<Renderer>().material = setColor(head.tag);
        body.transform.GetChild(0).GetComponent<Renderer>().material = setColor(body.tag);
        leftLeg.transform.GetChild(0).GetComponent<Renderer>().material = setColor(leftLeg.tag);
        rightLeg.transform.GetChild(0).GetComponent<Renderer>().material = setColor(rightLeg.tag);
        this.leftLeg = leftLeg;
    }

    Material setColor(string colorName)
    {
        if(colorName.Equals("red")) {
            return red;
        }
        else if(colorName.Equals("blue"))
        {
            return blue;
        }
        else if (colorName.Equals("yellow"))
        {
            return yellow;
        }
        else if (colorName.Equals("green"))
        {
            return green;
        }
        return purple;
    }

    public void endGame()
    {
        playing = false;
        gameOver = true;    
    }

    public void incrementLevel()
    {
        playing = false;
        level++;
        paused = true;
        if(toyClone != null)
        {
            Destroy(toyClone);
        }
        redTrigger.GetComponent<ColorTrigger>().resetCount();
        if(bombClone != null)
        {
            Destroy(bombClone);
        }
        if(powerUpClone != null)
        {
            Destroy(powerUpClone);
        }
        bombTimer = 10.0f;
        powerUpTimer = 20.0f;
        if (level == 2)
        {
            level2.SetActive(true);
        }
        else if (level == 3)
        {
            level3.SetActive(true);
        }
        else if (level == 4)
        {
            level4.SetActive(true);
        }
    }

    public void startLevel()
    {
        level2.SetActive(false);
        level3.SetActive(false);
        level4.SetActive(false);
        playing = true;
        paused = false;
        spawnToy();
    }
}
