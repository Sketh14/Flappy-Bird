using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public Text scoreToDisplay;
    public Text countDown;
    public Image getReady;
    public GameObject[] pipesToChange;
    public Transform[] startPoint;
    public Text currentScore;
    public Text highScore;
    public Transform coinContainer;
    public Transform scoreContainer;
    //public float[] pipeToggle = new float[2] { 4f, 0.025f };
    //public bool changeStats = false;

    private bool gameStarted = false;
    private int tempCountDown = 3;
    private int tempHighScore = 0;
    //[SerializeField]
    private int intToggle = 0;                  //for running method to particular count

    private int scoreOfPlayer = 0;
    public int ScoreOfPlayer
    {
        get
        {
            return scoreOfPlayer;
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        tempHighScore = SaveSystem1.LoadHighScore1(null, false);
    }

    void Update()
    {
        {       //not needed for now
            /*
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
            */
        }

        //increase difficulty
        if( (scoreOfPlayer != 0) && intToggle < 1 && ((scoreOfPlayer % 20) == 0 || (scoreOfPlayer % 50) == 0) )
        {
            intToggle++; 
            //changeStats = true;                  //not needed
            if((scoreOfPlayer % 50) == 0)          //PipeGap
            {
                //Debug.Log("changing stats : Position , Score : " + scoreOfPlayer + " , intToggle : " + intToggle);
                //pipeToggle[0] -= 0.1f;
                float changeValue = 0.1f;
                foreach(GameObject pipe in pipesToChange)
                {
                    pipe.GetComponent<PipeController>().ChangeStats(changeValue);           //0 for increasing Gap
                }
                StartCoroutine(WaitFor(4));
            }
            else                                //PipeSPeed
            {
                //Debug.Log("changing stats : Speed");
                //pipeToggle[1] += 0.01f;
                float changeValue = 0.01f;
                foreach (GameObject pipe in pipesToChange)
                {
                    pipe.GetComponent<PipeController>().ChangeStats(changeValue);           //1 for increasing Speed
                }
                StartCoroutine(WaitFor(4));
            }
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        scoreToDisplay.text = "0";
        CountDownTimer();

        {                 //not needed
            /*
            //GameManager.Instance.playerMain.GetComponent<PolygonCollider2D>().enabled = true;
            //transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            //countDown.gameObject.SetActive(true);
            //getReady.gameObject.SetActive(true);
            */ 
        }
    }

    public void LoadScore()             //to show score in Main Menu
    {
        int[] tempArray = { 0, 0, 0, 0 };                        //initialize empty array for containing score
        SaveSystem1.LoadHighScore1(tempArray, true);
        //if (tempArray != null)
        {
            for (int i = 0; i < 4; i++)
            {
                scoreContainer.GetChild(i).GetComponent<Text>().text = tempArray[i].ToString();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameStarted = false;
        GameManager.Instance.gameOver = true;
        GameManager.Instance.playerMain.PrepPlayer(0f, true);
        tempCountDown = 3;                          //reset tempCountDon for pause menu

        {                   //not needed
            /*
            //transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
            //GameManager.Instance.playerMain.playerRB.isKinematic = true;
            //GameManager.Instance.playerMain.playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            */
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameStarted = true;
        CountDownTimer();

        {         //not needed
            /* 
            //transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
            //countDown.gameObject.SetActive(true);
            //getReady.gameObject.SetActive(true);
            */
        }
    }

    public void MainMenu()
    {
        //call save script before loading Main Menu
        if (Time.timeScale == 0f)
        {
            //Debug.Log("Calling Save, MainMenu, MainScript");
            SaveSystem1.SaveHighScore1(scoreOfPlayer);
        }

        Time.timeScale = 1f;

        GameManager.Instance.playerMain.isSmashed = true;
        GameManager.Instance.playerMain.PrepPlayer(0f, false);
        {
            /*
            //GameManager.Instance.playerMain.playerRB.constraints = RigidbodyConstraints2D.None;
            //GameManager.Instance.playerMain.gameObject.SetActive(true);
            //GameManager.Instance.playerMain.GetComponent<PolygonCollider2D>().enabled = false;
            */
        }

        //for activating only Main Menu
        {
            Transform tempObject = transform.GetChild(0).transform;

            for (int i = 0; i < 4; i++)
            {
                tempObject.GetChild(i).gameObject.SetActive(false);        //Deactivate all
            }
            tempObject.GetChild(1).gameObject.SetActive(true);     //activate only Start Menu
        }

        //for restarting start Points from beginning
        {
           // Debug.Log(pipesToChange[0].transform.root.name);
            {   
                //did not fixed itself
                //pipe was not restarting correctly, parent object was not changing position
                int tempIndexValue = 0;
                for(int addValue = 0; addValue < 20; addValue += 4)
                {
                    pipesToChange[tempIndexValue].transform.position = startPoint[1].position + new Vector3(addValue, Random.Range(0f,4f), 0f);
                    tempIndexValue++;
                    //addValue += 4;
                }
                //pipesToChange[0].transform.root.localPosition = startPoint[1].position;
            }
            GameManager.Instance.playerMain.transform.position = startPoint[0].position;
        }
    }

    private void CountDownTimer()
    {
        if (tempCountDown > -1)
        {
            StartCoroutine(WaitFor(tempCountDown--));
        } 
    }

    private void ChooseMedal(int indexValue)
    {
        for(int i=0; i<3; i++)
        {
            coinContainer.GetChild(i).gameObject.SetActive(false);
        }
        coinContainer.GetChild(indexValue).gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gameStarted = false;
        tempCountDown = 3;
        scoreToDisplay.enabled = false; 
        transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(WaitFor(5));                   //wait some before activating Menu Button;
        transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        GameManager.Instance.playerMain.PrepPlayer(2f, false);

        //call save script
        //Debug.Log("Calling Save, GameOver, MainScript");
        SaveSystem1.SaveHighScore1(scoreOfPlayer);

        //display high score
        {
            currentScore.text = scoreOfPlayer.ToString();
            if (tempHighScore < scoreOfPlayer)
            {
                highScore.text = scoreOfPlayer.ToString(); 
            }
            else
            {
                highScore.text = tempHighScore.ToString(); 
            } 
        }

        //display medal
        {
            int index;
            if (scoreOfPlayer <= 20)
            {
                index = 0;
            }
            else if (scoreOfPlayer > 20 && scoreOfPlayer <= 50)
            {
                index = 1;
            }
            else
            {
                index = 2;
            }
            ChooseMedal(index);               //value not greater than 2 
        }

        //GameManager.Instance.playerMain.PrepPlayer(0f);
    }

    public void UpdateScore(int score)                    //Update score if collected
    {
        scoreOfPlayer += score;
        scoreToDisplay.text = scoreOfPlayer.ToString();
        //Debug.Log("Point Collected : " + scoreOfPlayer);
    }

    IEnumerator WaitFor(int n)                //return after specified second
    {
        if (n == 4)                           //for changing stats
        {
            yield return new WaitForSeconds(3f);
            intToggle = 0;
        }
        else if(n == 5)                      //activate Main Menu Button
        {
            if (GameManager.Instance.playerMain.isActiveAndEnabled == false)
            {
                //Debug.Log("Not Hit");
                yield return 0;
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }
            transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true); 
        }
        else                                  // for countDown sequence
        {
            yield return new WaitForSeconds(1f);
            //Debug.Log(n);
            if (countDown.text.Length != 1)
            {
                getReady.gameObject.SetActive(false);
            }
            countDown.text = n.ToString();
            if (n == 0)
            {
                yield return new WaitForSeconds(0.3f);
                //countDown.gameObject.SetActive(false);
                countDown.text = "";
                scoreOfPlayer = 0;
                if (gameStarted)
                {
                    scoreToDisplay.enabled = true;
                    GameManager.Instance.playerMain.isSmashed = false;              //only to start the game
                    GameManager.Instance.gameOver = false;
                    transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                    GameManager.Instance.playerMain.PrepPlayer(2f, false);
                    //GameManager.Instance.playerMain.playerRB.gravityScale = 2f;
                    //GameManager.Instance.playerMain.playerRB.constraints = RigidbodyConstraints2D.None;
                    //GameManager.Instance.playerMain.playerRB.isKinematic = false;
                }
            }
            CountDownTimer(); 
        }
    }
}
