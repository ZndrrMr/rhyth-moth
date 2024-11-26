using UnityEngine;

public class BugMovement : MonoBehaviour
{
    public float speed = 2f; // Movement speed of the bug

    private Vector2 direction; // Direction of movement

    private void Start()
    {
        // Calculate the direction toward the center of the screen (0, 0)
        Vector2 screenCenter = Vector2.zero; // Center of the screen
        direction = (screenCenter - (Vector2)transform.position).normalized; // Normalize to get direction

    }

    private void Update()
    {
        // Move the bug in the calculated direction
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        // Destroy the bug if it goes beyond the screen bounds
        if (transform.position.x > 10f || transform.position.x < -10f ||
            transform.position.y > 6f || transform.position.y < -6f)
        {
            // Uncomment to destroy the bug when it exits the screen
            Destroy(gameObject);
        }
    }
}