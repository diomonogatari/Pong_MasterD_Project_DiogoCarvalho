using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text player1ScoreText;
    public Text player2ScoreText;
    public GameObject ballPrefab;
    public Vector2 goalLimits;
    /*the x represents player1 goal
     *the y represents player2 goal
     */

    private BallScript ballScript;

    private ushort player1Score = 0;
    private ushort player2Score = 0;

    private bool didPlayer1Win;

    void Start()
    {
        Application.targetFrameRate = 60;

        ballScript = ballPrefab.GetComponent<BallScript>();

        player1ScoreText.text = player2ScoreText.text = "0";

    }

    void FixedUpdate()
    {
        if (CheckGoal(ballPrefab, out didPlayer1Win))
            ballScript.ResetBall(didPlayer1Win);
    }
    private bool CheckGoal(GameObject ball, out bool Player1Win)
    {
        if (ball.transform.position.x >= goalLimits.y)
        {
            player1Score++;
            player1ScoreText.text = player1Score.ToString();
            Player1Win = true;
            return true;
        }
        if (ball.transform.position.x <= goalLimits.x)
        {
            player2Score++;
            player2ScoreText.text = player2Score.ToString();
            Player1Win = false;
            return true;
        }
        Player1Win = false;
        return false;
    }
}
