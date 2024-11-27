using UnityEngine;

public class BugTap : MonoBehaviour
{
    private TapAreaManager tapAreaManager;
    public GameObject particleEffect; // Assign your particle prefab in the Inspector
    public static bool isBugBeingTapped = false; // Changed to public for access by BugTapManager

    private void Start()
    {
        // Find the TapAreaManager in the scene
        tapAreaManager = FindFirstObjectByType<TapAreaManager>();
        if (tapAreaManager == null)
        {
            Debug.LogError("TapAreaManager not found in the scene!");
        }
    }

    private void Update()
    {
        // Remove the spacebar input handling from here
    }

    // New method to attempt tapping this bug
    public bool TryTapBug()
    {
        if (IsInsideAnyTapRadius() && IsFurthestBug())
        {
            Debug.Log("Bug is inside a tap radius!");
            isBugBeingTapped = true;
            TapBug();
            return true;
        }
        return false;
    }

    private bool IsFurthestBug()
    {
        Vector2 thisPosition = transform.position;
        Vector2 thisDirection = GetMovementDirection();
        
        // Calculate how far along its path this bug is
        float thisProgressAlongPath = Vector2.Dot(thisPosition, thisDirection);

        // Get all other bugs
        BugTap[] allBugs = FindObjectsByType<BugTap>(FindObjectsSortMode.None);

        foreach (var otherBug in allBugs)
        {
            // Skip comparing to self
            if (otherBug == this) continue;

            Vector2 otherPosition = otherBug.transform.position;

            // Only compare against other bugs that are in the tap radius
            if (!tapAreaManager.IsInsideTapRadius(otherPosition, TapAreaManager.TapAreaType.Good))
            {
                continue;
            }

            // Calculate how far along its path the other bug is
            Vector2 otherDirection = otherBug.GetMovementDirection();
            float otherProgressAlongPath = Vector2.Dot(otherPosition, otherDirection);

            // If another bug in the radius is further along its path, this isn't the one to tap
            if (otherProgressAlongPath > thisProgressAlongPath)
            {
                return false;
            }
        }

        // If we got here, this bug is the furthest along its path in the radius
        return true;
    }

    private Vector2 GetMovementDirection()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.magnitude > 0.01f)
        {
            return rb.linearVelocity.normalized; // Return the normalized velocity vector
        }

        // Default to no movement if no Rigidbody2D or velocity is too low
        return Vector2.zero;
    }

    private bool IsInsideAnyTapRadius()
    {
        Vector2 bugPosition = transform.position;

        // Replace the old logic with TapAreaManager calls
        return tapAreaManager.IsInsideTapRadius(bugPosition, TapAreaManager.TapAreaType.Good) ||
               tapAreaManager.IsInsideTapRadius(bugPosition, TapAreaManager.TapAreaType.Nice) ||
               tapAreaManager.IsInsideTapRadius(bugPosition, TapAreaManager.TapAreaType.Excellent);
    }

    private void TapBug()
    {
        Debug.Log($"Bug tapped at position {transform.position}");

        // Instantiate the particle effect
        if (particleEffect != null)
        {
            // Get the direction the bug is moving
            Vector2 movementDirection = GetMovementDirection();

            // Check if the movement direction is valid (non-zero)
            if (movementDirection != Vector2.zero)
            {
                // Calculate the angle between the movement direction and the positive X-axis
                float angle = (Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg) - 90f;

                Debug.Log($"Particle effect angle: {angle} degrees");

                // Create a rotation based on the calculated angle
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                // Instantiate the particle effect with the calculated rotation
                GameObject effectInstance = Instantiate(particleEffect, transform.position, rotation);

                // Destroy the particle effect after its duration
                Destroy(effectInstance, effectInstance.GetComponent<ParticleSystem>().main.duration);
            }
        }

        // Destroy this bug
        Destroy(gameObject);

        // Reset the flag using BugTapManager
        BugTapManager.Instance.ResetTapFlagWithDelay(0.1f); // Adjust delay as needed
    }
}