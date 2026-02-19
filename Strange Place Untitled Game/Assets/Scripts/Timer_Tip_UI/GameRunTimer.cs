using UnityEngine;
using System.Collections.Generic;

public class GameRunTimer : MonoBehaviour
{
    public static GameRunTimer Instance;

    public List<SplitData> splits = new List<SplitData>();
    public System.Action<SplitData> OnSplitCompleted;

    private float totalTime = 0f;
    private float phaseStartTime = 0f;
    private bool isRunning = true;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (!isRunning) return;

        totalTime += Time.deltaTime;
    }

    public void StartRun()
    {
        totalTime = 0f;
        isRunning = true;
    }

    public void StartPhase(string phaseName)
    {
        phaseStartTime = totalTime;
    }

    public void CompletePhase(string phaseName)
    {
        float phaseTime = totalTime - phaseStartTime;

        SplitData split = splits.Find(s => s.phaseName == phaseName);

        if (split == null)
        {
            split = new SplitData();
            split.phaseName = phaseName;
            splits.Add(split);
        }

        split.lastTime = phaseTime;

        float previousBest = split.bestTime;

        if (split.bestTime == 0 || phaseTime < split.bestTime)
            split.bestTime = phaseTime;

        split.completed = true;

        OnSplitCompleted?.Invoke(split);
    }

    public float GetTotalTime()
    {
        return totalTime;
    }

    public void StopRun()
    {
        isRunning = false;
    }
}
