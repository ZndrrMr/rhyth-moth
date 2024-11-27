using UnityEngine;
using System.Collections;

public class BugTapManager : MonoBehaviour
{
    public static BugTapManager Instance { get; private set; }
    private MissCounter missCounter;
    private TapAreaManager tapAreaManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        missCounter = FindFirstObjectByType<MissCounter>();
        tapAreaManager = FindFirstObjectByType<TapAreaManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !BugTap.isBugBeingTapped)
        {
            // Check if any bug was successfully tapped
            bool successfulTap = false;
            BugTap[] allBugs = FindObjectsByType<BugTap>(FindObjectsSortMode.None);
            
            foreach (var bug in allBugs)
            {
                if (bug.TryTapBug())
                {
                    successfulTap = true;
                    break;
                }
            }

            // If no bug was tapped, count it as a miss
            if (!successfulTap)
            {
                missCounter.IncrementMissCounter();
            }
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