using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    public bool isSmashed = true; 
    public Rigidbody2D playerRB;
    public Transform startPoint;

    void Update()
    {
        // to avoid space input if game is paused and main menu key is pressed, time.timescale comparison
        // without this, when hitting main menu and continously pressing space launches player
        if (!isSmashed && (Time.timeScale != 0f))               
        {
            if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= 5f)
            {
                playerRB.AddForce(new Vector2(0f, 800f));                   //make the player jump
            } 
        }

        if(transform.position.y <= -5.5f)
        {
            gameObject.SetActive(false);
            if (!isSmashed)                    //to disable double activation
            {
                Smashed();
            }
            //transform.position = startPoint.position;
        }
    }

    //not needed because of RBconstraint
    public void PrepPlayer(float changeValue, bool freeze)
    {
        playerRB.gravityScale = changeValue;
        if (freeze)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        playerRB.constraints = RigidbodyConstraints2D.None;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))                   //check if player hit a pipe, works
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            //Debug.Log("Hit!!" + collision.name);
            Smashed();
            isSmashed = true;
        }

        if (collision.CompareTag("Point"))
        {
            //Debug.Log("Point Collected");
            GameManager.Instance.mainScript.UpdateScore(1);
        }
    }

    private void Smashed()
    {
        GameManager.Instance.gameOver = true;
        if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Hit Space");
            playerRB.AddForce(new Vector2(0f, 300f));                //to avoid extra bounce, needs work

        }
        else             //normal add force
        {
            playerRB.AddForce(new Vector2(0f, 700f));             //If hit apply force up 
        }
        GameManager.Instance.mainScript.GameOver(); 
    }
}
