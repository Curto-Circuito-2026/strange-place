using UnityEngine;
using System.Collections.Generic;

public class GameRunTimer : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public BalanceUI balance;
    public static GameRunTimer Instance;

    public List<SplitData> splits = new List<SplitData>();
    public System.Action<SplitData> OnSplitCompleted;

    [SerializeField] private TipUI tipUI;

    private float totalTime = 0f;
    private float phaseTimeSpeedrun = 0f;
    private float phaseTimeTip = 0f;
    private float phaseStartTimeSpeedrun = 0f;
    private float phaseStartTimeTip = 0f;
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
        
        //Debug.Log(phaseTimeSpeedrun);
        //Debug.Log(phaseTimeTip);
        
        if (!isRunning)
        {
            Debug.Log("Not Running");
            return;

        }
        
        totalTime += Time.deltaTime;
        phaseTimeSpeedrun = totalTime - phaseStartTimeSpeedrun;
        phaseTimeTip = totalTime - phaseStartTimeTip;
    }

    public void StartRun()
    {
        totalTime = 0f;
        isRunning = true;
        Debug.Log("Running");
        canvas.SetActive(true);
    }

    public void StartPhase(string phaseName)
    {
        phaseStartTimeSpeedrun = totalTime;
    }

    public void CompletePhase(string phaseName)
    {
        
        phaseStartTimeTip = totalTime;
        
        SplitData split = splits.Find(s => s.phaseName == phaseName);

        if (split == null)
        {
            split = new SplitData();
            split.phaseName = phaseName;
            splits.Add(split);
        }

        split.lastTime = phaseTimeSpeedrun;

        float previousBest = split.bestTime;

        if (split.bestTime == 0 || phaseTimeSpeedrun < split.bestTime)
            split.bestTime = phaseTimeSpeedrun;

        split.completed = true;

        OnSplitCompleted?.Invoke(split);
        
        tipUI.CalculateTip(phaseTimeTip);
    }

    public float GetTotalTime()
    {
        return totalTime;
    }
    public float GetPhaseTime()
    {
        return phaseTimeTip;
    }
    public void StopRun()
    {
        canvas.SetActive(false);
        isRunning = false;
    }
}
