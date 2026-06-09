using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] Rigidbody2D[] ballPrefabs;
    [SerializeField] Transform target;
    [SerializeField] float radius = 3f;
    [SerializeField] float initialForce = 8f;
    Rigidbody2D[] spawnedBalls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBallsInCircle();
        // InitialPush();
    }

    public void SpawnBallsInCircle()
    {
        spawnedBalls = new Rigidbody2D[ballPrefabs.Length];

        for (int i = 0; i < ballPrefabs.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / ballPrefabs.Length;

            Vector2 spawnPosition =
                (Vector2)target.position +
                new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            Rigidbody2D ball =
                Instantiate(ballPrefabs[i], spawnPosition, Quaternion.identity);

            spawnedBalls[i] = ball;
        }
    }

    public void InitialPush()
    {
        foreach (Rigidbody2D rb in spawnedBalls)
        {
            Vector2 targetPosition = target.position;
            Vector2 direction = (rb.position - targetPosition).normalized;

            rb.AddForce(direction * initialForce, ForceMode2D.Impulse);
        }
    }

    public void ClearBalls()
    {
        spawnedBalls.ToList().ForEach(ball => Destroy(ball.gameObject));
    }
}