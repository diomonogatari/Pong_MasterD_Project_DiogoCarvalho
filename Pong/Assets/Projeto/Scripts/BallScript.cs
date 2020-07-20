using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BallScript : MonoBehaviour
{

    #region Public Variables

    /* I don't understand these min max velocities
    * The min velocity will be the launch speed. Pong gets harder every bump or else it's a random game. If it's random, might as well just randomly shoot the ball to make it a game of chance and not skill
    * The max velocity at this level is either too slow and makes the game infinite for 2 high skill players or it's high enough to make them lose. The "sweet spot" of high enough and "game breaking speed" is almost the same
    * I decided that these limiters don't help in anything in this game.
    */
    //public float minYVelocity = 5.0f;
    //public float maxYVelocity = 5.0f;
    //public float minXVelocity = 5.0f;
    //public float maxXVelocity = 5.0f;
    [Range(0, 10)]
    public float initialSpeed = 3.0f;

    [Range(0, 5)]
    public float difficultyMultiplier = 0.2f;/*Increases speed by 20%*/

    public bool isDebug = false;

    #endregion

    #region Private Varriables

    private Rigidbody2D rgdBall;
    private float radius;
    private Vector2 direction;
    private Vector3 startPosition;
    private bool? isPlayer1Serving;
    private AudioSource bumpingBeep;

    private bool? isPaddleMovingUp = null; /*null is not moving*/
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rgdBall = this.GetComponent<Rigidbody2D>();
        direction = GetMovingDirection(null);
        radius = transform.localScale.x / 2;//half the width

        bumpingBeep = GetComponent<AudioSource>();

        /*Saves position to use as a reset point*/
        startPosition = rgdBall.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(direction * Time.fixedDeltaTime);
    }

    private Vector2 GetMovingDirection(bool? didPlayer1Win)
    {

        int directionSignal;
        if (didPlayer1Win == null)
        {
            /*cast to int to have absolute values*/
            var r = (int)System.Math.Round(Random.value);
            directionSignal = r == 0 ? -1 : r;
        }
        else if (didPlayer1Win == true)/*if true*/
        {
            /*positive direction on X sends the ball to player 2 side*/
            directionSignal = 1;
        }
        else
        {
            /*while negative direction on X the ball goes to player 1*/
            directionSignal = -1;
        }
        /*Send the ball straight if I'm debuging stuff*/
        if (isDebug)
            return new Vector2(initialSpeed * directionSignal, 0 * initialSpeed);

        float initialYValue = Random.Range(-3.0f, 3.0f);
        /*if the value is between 0 and 1*/
        if (initialYValue >= 0 && initialYValue <= 1)
        {
            initialYValue = 2.0f;
        }
        /*if the value is between -1 and 0*/
        else if (initialYValue >= -1 && initialYValue <= 0)
        {
            initialYValue = -2.0f;
        }
        return new Vector2(initialSpeed * directionSignal, initialYValue * initialSpeed);
        //return new Vector2(initialSpeed * directionSignal, 0 * initialSpeed);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Paddle 1" || collider.gameObject.name == "Paddle 2")
        {
            var script = collider.gameObject.GetComponent<PaddleMovement>();
            if (script.GetIsPlayer1())
            {
                if (script.GetIsPaddle1NotMoving())
                    isPaddleMovingUp = null;
                else
                    isPaddleMovingUp = script.GetIsPaddle1MovingUp();
            }
            else
            {
                if (script.GetIsPaddle2NotMoving())
                    isPaddleMovingUp = null;
                else
                    isPaddleMovingUp = script.GetIsPaddle2MovingUp();
            }
        }


        switch (collider.tag)
        {
            case "Paddle":
                direction = RecalculateDirectionAfterPaddleBounce(direction);
                bumpingBeep.pitch = Random.Range(0.95f, 1.35f);
                bumpingBeep.Play();
                break;
            case "World":
                direction = RecalculateDirectionAfterWorldBounce(direction);
                bumpingBeep.pitch = Random.Range(0.98f, 1.05f);
                bumpingBeep.Play();
                break;
            case "GoalLeft":
                Debug.Log("Player 2 scores");
                ResetBall(false);
                break;
            case "GoalRight":
                Debug.Log("Player 1 scores");
                ResetBall(true);
                /*TODO: call score manager and add score */
                break;
        }
    }
    private Vector2 RecalculateDirectionAfterWorldBounce(Vector2 movingDirection)
    {
        return new Vector2(movingDirection.x, movingDirection.y * -1);
    }
    private Vector2 RecalculateDirectionAfterPaddleBounce(Vector2 movingDirection)
    {
        var speedIncreaseX = movingDirection.x * difficultyMultiplier * -1;
        var speedIncreaseY = movingDirection.y * difficultyMultiplier;

        bool isGoingOppositeY = false;

        /*if the ball and the paddle are going the same direction I want to give a boost on Y speed*/
        if ((movingDirection.y < 0 && isPaddleMovingUp == false) || (movingDirection.y > 0 && isPaddleMovingUp == true))
            isGoingOppositeY = false;
        /*If they are moving opposite I want to switch the Y direction and not give a boost*/
        if ((movingDirection.y < 0 && isPaddleMovingUp == true) || (movingDirection.y > 0 && isPaddleMovingUp == false))
            isGoingOppositeY = true;

        if (isPaddleMovingUp == true)
            speedIncreaseY += 1.0f;
        else if (isPaddleMovingUp == false)
            speedIncreaseY -= 1.0f;
        if (isGoingOppositeY)                                           /*just a bit of speed reduc*/
            return new Vector2(movingDirection.x * -1 + speedIncreaseX, (movingDirection.y - 1.0f)*-1);
        else
            return new Vector2(movingDirection.x * -1 + speedIncreaseX, movingDirection.y + speedIncreaseY);
    }
    public void ResetBall(bool player1Win)
    {
        /*reset the position*/
        rgdBall.transform.position = startPosition;
        /*reset the speed*/
        direction = GetMovingDirection(player1Win);

    }
}
