using TMPro;
using UnityEngine;

public class TipUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI totalText;

    private float totalTips = 0f;

    public void AddTip(float value)
    {
        totalTips += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        totalText.text = "$ " + totalTips.ToString("F2");

        if (totalTips >= 500)
            statusText.text = "Máximo";
        else if (totalTips >= 300)
            statusText.text = "Alto";
        else if (totalTips >= 150)
            statusText.text = "Médio";
        else if (totalTips > 0)
            statusText.text = "Pouco";
        else
            statusText.text = "Nada";
    }
}
