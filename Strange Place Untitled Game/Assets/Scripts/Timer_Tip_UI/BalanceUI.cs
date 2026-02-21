using UnityEngine;

public class BalanceUI : MonoBehaviour
{
    [SerializeField] private RectTransform pointer;
    [SerializeField] private RectTransform bar;

    [Range(-1f, 1f)]
    public float balanceValue;

    public void UpdateBalance(float value)
    {
        balanceValue = Mathf.Clamp(value, -1f, 1f);

        float halfWidth = bar.rect.width / 2f;
        float targetX = balanceValue * halfWidth;

        pointer.anchoredPosition = new Vector2(targetX, pointer.anchoredPosition.y);
    }
}