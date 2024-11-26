using UnityEngine;

public class TapAreaManager : MonoBehaviour
{
    [SerializeField] private float goodTapRadius = 1f;
    [SerializeField] private float niceTapRadius = 0.8f;
    [SerializeField] private float excellentTapRadius = 0.5f;

    public enum TapAreaType
    {
        Good,
        Nice,
        Excellent
    }

    public bool IsInsideTapRadius(Vector2 position, TapAreaType areaType)
    {
        float tapRadius = GetTapRadius(areaType);
        float distance = Vector2.Distance(transform.position, position);


        return distance <= tapRadius;
    }

    private float GetTapRadius(TapAreaType areaType)
    {
        switch (areaType)
        {
            case TapAreaType.Good:
                return goodTapRadius;
            case TapAreaType.Nice:
                return niceTapRadius;
            case TapAreaType.Excellent:
                return excellentTapRadius;
            default:
                return 0f;
        }
    }
}