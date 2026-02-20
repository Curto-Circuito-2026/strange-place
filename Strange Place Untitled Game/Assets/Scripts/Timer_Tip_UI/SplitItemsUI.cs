using TMPro;
using UnityEngine;

public class SplitItemUI : MonoBehaviour
{
    public TextMeshProUGUI phaseNameText;
    public TextMeshProUGUI timeText;

    public void Setup(string phaseName, float time, float bestTime)
    {
        phaseNameText.text = phaseName;

        string formattedTime = FormatTime(time);

        if (bestTime > 0)
        {
            float delta = time - bestTime;

            string sign = delta <= 0 ? "-" : "+";
            string deltaText = sign + FormatTime(Mathf.Abs(delta));

            timeText.text = formattedTime + "  (" + deltaText + ")";

            timeText.color = delta <= 0 ? Color.green : Color.red;
        }
        else
        {
            timeText.text = formattedTime;
            timeText.color = Color.white;
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
