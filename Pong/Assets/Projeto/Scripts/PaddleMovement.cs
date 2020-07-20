using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public GameObject paddle;
    public float forceMultiplier = 5.0f;
    public bool isPlayer1;



    private Rigidbody2D rbPaddle;
    private bool isPaddle1MovingUp;
    private bool isPaddle2MovingUp;

    private bool isPaddle1NotMoving;
    private bool isPaddle2NotMoving;

    private void Start()
    {
        rbPaddle = paddle.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Player1CheckMovement();
        Player2CheckMovement();
    }

    #region Player1 methods
    private void Player1CheckMovement()
    {
        if (Input.GetButton("Vertical1") && isPlayer1)
        {
            Player1Move();
            isPaddle1NotMoving = false;
        }
        else
            isPaddle1NotMoving = true;
    }
    private void Player1Move()
    {
        if (Input.GetAxis("Vertical1") < 0)
            //Going down
            isPaddle1MovingUp = false;
        else if (Input.GetAxis("Vertical1") > 0)
            //Going up
            isPaddle1MovingUp = true;

        rbPaddle.transform.position += Input.GetAxis("Vertical1") * transform.up * forceMultiplier * Time.deltaTime;
    }
    #endregion

    #region Player2 Methods

    private void Player2CheckMovement()
    {
        if (Input.GetButton("Vertical2") && !isPlayer1)
        {
            Player2Move();
            isPaddle2NotMoving = false;
        }
        else
            isPaddle2NotMoving = true;
    }

    private void Player2Move()
    {
        if (Input.GetAxis("Vertical2") < 0)
            //Going down
            isPaddle2MovingUp = false;
        else if (Input.GetAxis("Vertical2") > 0)
            //Going up
            isPaddle2MovingUp = true;
        rbPaddle.transform.position += Input.GetAxis("Vertical2") * transform.up * forceMultiplier * Time.deltaTime;

    }
    #endregion

    public bool GetIsPlayer1()
    {
        return isPlayer1;
    }

    public bool GetIsPaddle1MovingUp()
    {
        return isPaddle1MovingUp;
    }
    public bool GetIsPaddle1NotMoving()
    {
        return isPaddle1NotMoving;
    }

    public bool GetIsPaddle2MovingUp()
    {
        return isPaddle2MovingUp;
    }
    public bool GetIsPaddle2NotMoving()
    {
        return isPaddle2NotMoving;
    }
}
