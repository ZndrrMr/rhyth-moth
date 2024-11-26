using UnityEngine;
using System.Collections;

public class BugTapManager : MonoBehaviour
{
    public static BugTapManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure there's only one instance of BugTapManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetTapFlagWithDelay(float delay)
    {
        StartCoroutine(ResetTapFlagCoroutine(delay));
    }

    private IEnumerator ResetTapFlagCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        BugTap.isBugBeingTapped = false;
    }
}