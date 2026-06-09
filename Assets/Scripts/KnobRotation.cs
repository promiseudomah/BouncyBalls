using UnityEngine;

public class KnobRotation : MonoBehaviour
{
    [Header("Particle Rotation Speed")]
    [SerializeField] private float rotateSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, -1 * rotateSpeed * Time.deltaTime);
    }
}