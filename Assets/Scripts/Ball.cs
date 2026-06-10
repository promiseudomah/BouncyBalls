using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // public int teamId;

    public enum Team
    {
        A,
        B
    }

    public Team team;

    [SerializeField] float forceStrength;
    [SerializeField] float maxSpeed, minSpeed;
    [SerializeField] float bounceForce = 10f;
    public float wallBounceSpeed;
    private PolygonCollider2D torusCollider;
    public float minY = 1f;
    public float attractRadius = 1.5f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(gameObject.name + ": Active");
        torusCollider = GameObject.Find("Torus").GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        FaceDirection();
        CapSpeed();
        AttractTowardsBounds();
    }

    private void FaceDirection()
    {
        if(rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void CapSpeed()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed > maxSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxSpeed;
        }
        
        Debug.Log(rb.linearVelocity);
    }

    private void AttractTowardsBounds()
    {
        if (rb.position.y >= minY)
        {
            Vector2 closestPoint = torusCollider.ClosestPoint(rb.position);
            Vector2 direction = (closestPoint - rb.position);

            float distance = direction.magnitude;
            if(distance <= attractRadius && distance > 0.01f)
            {
                Vector2 attractionForce = direction.normalized * forceStrength;
                rb.AddForce(attractionForce, ForceMode2D.Force);
                Debug.LogWarning("Attracting ball towards torus bound with force: " + attractionForce);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Vector2 bounceDir = rb.linearVelocity.normalized;
            rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);
            GameController.Instance.PlayCollisionSound();
        }

        if (other.gameObject.CompareTag("Bounds"))
        {
            Vector2 normal = rb.position.normalized;
            rb.linearVelocity =  rb.linearVelocity.normalized * wallBounceSpeed;
            GameController.Instance.PlayCollisionSound();
        }        
        
        if (other.gameObject.CompareTag("Net"))
        {
            GameController.Instance.OnGoalScored(this);
            GameController.Instance.PlayCollisionSound();
        }

        if(rb.position.y > 0.9f)
        {
            rb.AddForce(Vector2.up * forceStrength, ForceMode2D.Force);
        }
    }
}