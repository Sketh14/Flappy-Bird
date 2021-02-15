using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MainScript mainScript;
    public PlayerController playerMain;
    public bool gameOver = true;
    //public bool changeStats = false;
    //public float[] pipeToggle = new float[2] { 4f,0.025f};
    /*****************************
     * pipeToggle[0]               //pipeGap
     * pipeToggle[1]               //pipeSpeed
     ******************************/
     /*
    public float pipeGap = 4f;
    public float pipeSpeed = 0.025f;
    */

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
