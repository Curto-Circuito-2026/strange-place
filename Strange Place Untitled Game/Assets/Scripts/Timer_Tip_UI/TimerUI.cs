using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;

    void Update()
    {
        if (GameRunTimer.Instance == null) return;

        float time = GameRunTimer.Instance.GetTotalTime();
        totalTimeText.text = FormatTime(time);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
