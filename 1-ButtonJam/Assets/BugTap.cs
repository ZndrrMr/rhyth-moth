using UnityEngine;

public class BugTap : MonoBehaviour
{
    private TapAreaManager tapAreaManager;
    public GameObject particleEffect; // Assign your particle prefab in the Inspector
    private float tapRadius = .5f; // Fixed radius for tappable bugs
    public static bool isBugBeingTapped = false; // Changed to public for access by BugTapManager

    private void Start()
    {
        // Find the TapAreaManager in the scene
        tapAreaManager = FindObjectOfType<TapAreaManager>();
        if (tapAreaManager == null)
        {
            Debug.LogError("TapAreaManager not found in the scene!");
        }
    }

    private void Update()
{
    // Check if the spacebar is pressed and no bug is currently being tapped
    if (Input.GetKeyDown(KeyCode.Space) && !isBugBeingTapped && tapAreaManager != null)
    {
        if (IsInsideAnyTapRadius() && IsFurthestBug())
        {
            Debug.Log("Bug is inside a tap radius!");
            isBugBeingTapped = true; // Lock the tap
            TapBug();
        }
    }
}

    private bool IsFurthestBug()
    {
        // Get all BugTap instances in the scene
        BugTap[] allBugs = FindObjectsOfType<BugTap>();

        // Filter bugs that are within the tap radius
        var bugsInRadius = System.Linq.Enumerable.Where(allBugs, bug =>
        {
            if (bug == this) return false; // Exclude this bug
            float distance = Vector2.Distance(bug.transform.position, Vector2.zero);
            return distance <= tapRadius; // Only include bugs within the radius
        });

        // Calculate this bug's distance from the origin
        float thisDistance = Vector2.Distance(transform.position, Vector2.zero);

        // Get this bug's movement direction
        Vector2 thisDirection = GetMovementDirection();

        // Check if any bug in the radius is further in its respective direction
        foreach (var bug in bugsInRadius)
        {
            // Get the other bug's distance from the origin
            float otherDistance = Vector2.Distance(bug.transform.position, Vector2.zero);

            // Get the other bug's movement direction
            Vector2 otherDirection = bug.GetMovementDirection();

            // Compare the relative distance in the direction of movement
            float thisDot = Vector2.Dot(transform.position.normalized, thisDirection);
            float otherDot = Vector2.Dot(bug.transform.position.normalized, otherDirection);

            if (otherDistance > thisDistance && otherDot > thisDot)
            {
                // Another bug is further in its own direction
                return false;
            }
        }

        // If no other bug is further in its respective direction, this bug is the furthest
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