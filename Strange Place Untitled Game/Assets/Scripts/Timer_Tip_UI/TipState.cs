using UnityEngine;
using TMPro;

public class TipUI : MonoBehaviour
{
    [SerializeField] private TipConfig tipConfig;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI totalText;

    private int totalMoney;
    void Update()
    {
        float currentTime = GameRunTimer.Instance.GetPhaseTime();
        PreviewTip(currentTime);
        //Debug.Log(currentTime); // Debug timer
    }
    public void PreviewTip(float time)
    {
        string status = "";

        if (time < tipConfig.maxTime)
            status = "M�ximo";
        else if (time < tipConfig.highTime)
            status = "Alto";
        else if (time < tipConfig.mediumTime)
            status = "M�dio";
        else if (time < tipConfig.lowTime)
            status = "Baixo";
        else
            status = "Nada";

        statusText.text = $"Tip Atual: {status}";
    }
    public void CalculateTip(float completionTime)
    {
        int earned = 0;
        string status = "";

        if (completionTime < tipConfig.maxTime)
        {
            earned = tipConfig.maxTip;
            status = "M�ximo";
        }
        else if (completionTime < tipConfig.highTime)
        {
            earned = tipConfig.highTip;
            status = "Alto";
        }
        else if (completionTime < tipConfig.mediumTime)
        {
            earned = tipConfig.mediumTip;
            status = "M�dio";
        }
        else if (completionTime < tipConfig.lowTime)
        {
            earned = tipConfig.lowTip;
            status = "Baixo";
        }
        else
        {
            earned = tipConfig.noTip;
            status = "Nada";
        }

        totalMoney += earned;

        statusText.text = $"Tip: {status} (+{earned})";
        totalText.text = $"Total: {totalMoney}";
    }
}