using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI comboText;
    
    // Animation settings
    private readonly float popInScale = 1.5f;
    private readonly float popInDuration = 0.2f;
    private readonly float breakAnimDuration = 0.5f;

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

        // Initial setup
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
            // Set your preferred font asset in the inspector
            comboText.fontStyle = FontStyles.Bold;
            comboText.alignment = TextAlignmentOptions.Center;
        }
    }

    public void UpdateComboText(int combo)
    {
        if (combo > 1)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = combo.ToString();
            
            // Change color and size based on combo
            if (combo > 50)
            {
                comboText.color = Color.red;
                AnimateComboText(2f);
            }
            else if (combo > 25)
            {
                comboText.color = Color.yellow;
                AnimateComboText(1.75f);
            }
            else if (combo > 10)
            {
                comboText.color = Color.white;
                AnimateComboText(1.5f);
            }
        }
        else
        {
            comboText.gameObject.SetActive(false);
        }
    }

    private void AnimateComboText(float scale)
    {
        comboText.transform.DOKill(); // Kill any ongoing animations
        
        // Pop in animation using our defined variables
        comboText.transform.localScale = Vector3.one;
        comboText.transform.DOPunchScale(Vector3.one * popInScale * scale, popInDuration, 1, 0.5f)
            .SetEase(Ease.OutElastic);
    }

    private void AnimateComboBreak()
    {
        comboText.transform.DOKill();
        
        // Break animation sequence using our defined duration
        Sequence breakSequence = DOTween.Sequence();
        
        breakSequence.Append(comboText.transform.DOShakePosition(breakAnimDuration, 30, 20, 90, false, true));
        breakSequence.Join(comboText.material.DOColor(new Color(1, 0, 0, 1), breakAnimDuration * 0.5f));
        breakSequence.Join(comboText.transform.DOScale(0, breakAnimDuration).SetEase(Ease.InBack));
        
        breakSequence.OnComplete(() => {
            comboText.gameObject.SetActive(false);
            comboText.transform.localScale = Vector3.one;
        });
    }
}