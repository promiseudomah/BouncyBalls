using System;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private BallSpawner ballSpawner;
    float teamA_SCore;
    float teamB_Score;
    [SerializeField] TextMeshProUGUI scoreA_Text;
    [SerializeField] TextMeshProUGUI scoreB_Text;

    /// Awake is called when the script instance is being loaded.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballSpawner = FindFirstObjectByType<BallSpawner>();
    }

    public void OnGoalScored(Ball ball)
    {
        if (ball.team == Ball.Team.A)
        {
            teamA_SCore++;
        }
        else
        {
            teamB_Score++;
        }

        // Update this block if we will have more balls in the future

        UpdateUI();
        OnScoreContinue();
    }

    public void UpdateUI()
    {
        scoreA_Text.text = teamA_SCore.ToString();
        scoreB_Text.text = teamB_Score.ToString();
    }

    public void OnScoreContinue()
    {
        ballSpawner.ClearBalls();
        ballSpawner.SpawnBallsInCircle();
        // ballSpawner.InitialPush();
    }

    public void Restart()
    {
        Debug.Log("Restart!");
    }
}