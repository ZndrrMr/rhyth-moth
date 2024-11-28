using UnityEngine;
using System.Collections;

public class BugTapManager : MonoBehaviour
{
    public static BugTapManager Instance { get; private set; }
    private MissCounter missCounter;
    private TapAreaManager tapAreaManager;
    
    private int currentCombo = 0;
    public int CurrentCombo => currentCombo;

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
            bool successfulTap = false;
            BugTap[] allBugs = FindObjectsByType<BugTap>(FindObjectsSortMode.None);
            
            foreach (var bug in allBugs)
            {
                if (bug.TryTapBug())
                {
                    successfulTap = true;
                    IncrementCombo();
                    break;
                }
            }

            if (!successfulTap)
            {
                missCounter.IncrementMissCounter();
                ResetCombo();
            }
        }
    }

    private void IncrementCombo()
    {
        currentCombo++;
        Debug.Log($"Combo: {currentCombo}");
        UIManager.Instance.UpdateComboText(currentCombo);
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        Debug.Log("Combo Reset!");
        UIManager.Instance.UpdateComboText(currentCombo);
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