using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    public Transform[] containedPipes;
    public bool gameOver;

    [SerializeField]
    private float speedOfPipes = 0.025f;
    [SerializeField]
    private float gapBetweenPipes = 4f;

    //private bool isDown;                        //is Up or Down
    //private int heightValue = 0;
    //private int intTest = 0;                      // test variable to see if each script is updated
      
    void FixedUpdate()
    {
        //if(GameManager.Instance.changeStats)
        {
            //ChangeStats(GameManager.Instance.startScript.ScoreOfPlayer); 
            //UpdateTest();
            //GameManager.Instance.changeStats = false;
        }

        //check if game is over or not 
        if (!GameManager.Instance.gameOver)
        {
            transform.Translate(new Vector3( -speedOfPipes, 0f, 0f));          // speedOfPipes is -0.025

            if (transform.position.x < -9f)                 //works
            {
                //Debug.Log("Resetting Position");
                ResetPosition();
            } 
        }
    }

    private void ResetPosition()                          //reset position of Pipe if it reaches the end of screen
    {
        transform.position = new Vector3(11f, Random.Range(0f,4f), -0.2f); 
    }

    public void ChangeStats(float changeValue)
    {
        {
            /*         //old code
            if ((tempScorePlayer % 20) == 0)
            {
                gapBetweenPipes -= 0.1f;
            }
            else
            {
                speedOfPipes += 0.01f;
            }

        //GameManager.Instance.changeStats = false;
        //speedOfPipes = GameManager.Instance.pipeToggle[1];
        //gapBetweenPipes = GameManager.Instance.pipeToggle[0];

        //float tempGapContainer = gapBetweenPipes;
            */
        }

        if (changeValue == 0.1f)
        {
            //Debug.Log("Changing Position");
            gapBetweenPipes -= changeValue;
            foreach (Transform pipe in containedPipes)
            {
                if (pipe.position.y > 0f)
                {
                    changeValue *= -1;
                }
                pipe.position += new Vector3(0f, changeValue, 0f);
                changeValue *= -1;
            } 
        }
        else
        {
            speedOfPipes += changeValue;
            GameManager.Instance.mainScript.pipeSpeed = speedOfPipes;
        }
    }

    //Important for updating all Prefabs
    /*     //test went well
    private void UpdateTest()
    {
        GameObject[] myPipePrefab = GameObject.FindGameObjectsWithTag("Prefab");
        foreach (var prefab in myPipePrefab)
            prefab.GetComponent<PipeController>().intTest++;
        //prefab.intTest++;
        Debug.Log("I am Updated" + transform.name);
    }
    */

    /*          //old function for single pipes
    private void ResetPosition()
    {
        if(isDown)
        {
            gapBetweenPipes *= -1;
            //Debug.Log(gapBetweenPipes + transform.name);
        }
        transform.position = new Vector2(9.23f, gapBetweenPipes);

        //without this, the position is jumbled up if it comes back after a round, i.e. gap flips to (-) in 1st
        // round, then when it comes back its still (-), so need to flip it back for next round
        if (isDown)
        {
            gapBetweenPipes *= -1; 
        }

            //Debug.Log(transform.eulerAngles.y + transform.name);
    } 
    */
}
