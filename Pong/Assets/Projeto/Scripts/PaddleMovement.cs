using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public GameObject paddle;
    public float forceMultiplier = 5.0f;
    public bool isPlayer1;
    private Rigidbody2D rbPaddle;
    // Start is called before the first frame update
    private void Start()
    {
        rbPaddle = paddle.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {

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
        }
        if (Input.GetButton("Vertical1") && isPlayer1)
        {
            Player1Move();
        }
    }
    private void Player1Move()
    {
        rbPaddle.AddForce(Input.GetAxis("Vertical1") * transform.up * forceMultiplier * Time.deltaTime, ForceMode2D.Impulse);
    }
    #endregion

    #region Player2 Methods

    private void Player2CheckMovement()
    {
        if (Input.GetButton("Vertical2") && !isPlayer1)
        {
            Player2Move();
        }
        if (Input.GetButton("Vertical2") && !isPlayer1)
        {
            Player2Move();
        }
    }

    private void Player2Move()
    {
        rbPaddle.AddForce(Input.GetAxis("Vertical2") * transform.up * forceMultiplier * Time.deltaTime, ForceMode2D.Impulse);
    }
    #endregion

}
