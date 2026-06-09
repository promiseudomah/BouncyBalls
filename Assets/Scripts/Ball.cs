using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int teamId;
    public enum Team
    {
        A,
        B
    }
        
    public Team team;
    [SerializeField] float arenaRadius = 1.7f;
    [SerializeField] float attractionDistance = 0.5f;
    [SerializeField] float attractionForce = 3f;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(this.gameObject.name + ": Active");
    }

    void FixedUpdate()
    {
        float distanceFromCenter = Vector2.Distance(rb.position, Vector2.zero);

        float distanceToEdge = arenaRadius - distanceFromCenter;

        if (distanceToEdge <= attractionDistance)
        {
            Vector2 directionToCenter =
                ((Vector2)Vector2.zero - rb.position).normalized;

            rb.AddForce(directionToCenter * attractionForce);
        }
    }

    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Collided with another ball!");
        }

        if(other.gameObject.CompareTag("Net"))
        {
            GameController.Instance.OnGoalScored(this);
        }
    }
}