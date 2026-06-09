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

    [SerializeField] float arenaRadius = 1.7f;
    [SerializeField] float attractionDistance = 0.5f;
    [SerializeField] float attractionForce = 3f;
    [SerializeField] float maxSpeed, minSpeed;
    [SerializeField] float bounceForce = 10f;
    [SerializeField] float directionRandomness = 0.3f;
    [SerializeField] float forceRandomness = 0.3f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(gameObject.name + ": Active");
    }

    void FixedUpdate()
    {
        // float distanceFromCenter = Vector2.Distance(rb.position, Vector2.zero);
        // float distanceToEdge = arenaRadius - distanceFromCenter;

        // if (distanceToEdge <= attractionDistance)
        // {
        //     Vector2 directionToCenter =
        //         ((Vector2)Vector2.zero - rb.position).normalized;

        //     rb.AddForce(directionToCenter * attractionForce);
        // }

        FaceDirection();
        CapSpeed();
        Debug.Log(rb.linearVelocity);
    }

    void FaceDirection()
    {
        if(rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void CapSpeed()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed > maxSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D contact = other.GetContact(0);

        Vector2 bounceDir = Vector2.Reflect(
            rb.linearVelocity.normalized,
            contact.normal);

        bounceDir += Random.insideUnitCircle * directionRandomness;
        bounceDir.Normalize();

        float speed = Mathf.Clamp(
            rb.linearVelocity.magnitude,
            minSpeed,
            maxSpeed);

        rb.linearVelocity = bounceDir * speed;

        rb.AddForce(
            bounceDir * (bounceForce * 0.25f),
            ForceMode2D.Impulse);

        if (other.gameObject.CompareTag("Net"))
        {
            GameController.Instance.OnGoalScored(this);
        }
    }
}