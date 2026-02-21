using UnityEngine;
using System.Collections.Generic;

public class GameRunTimer : MonoBehaviour
{
    public static GameRunTimer Instance;

    public List<SplitData> splits = new List<SplitData>();
    public System.Action<SplitData> OnSplitCompleted;

    [SerializeField] private TipUI tipUI;

    private float totalTime = 0f;
    private float phaseTime = 0f;   //novo timer
    private bool isRunning = true;
    private bool isPhaseRunning = false;

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

        if (isPhaseRunning)
            phaseTime += Time.deltaTime;
    }

    public void StartRun()
    {
        totalTime = 0f;
        isRunning = true;
    }

    public void StartPhase(string phaseName)
    {
        phaseTime = 0f;        //reseta
        isPhaseRunning = true;
    }

    public void CompletePhase(string phaseName)
{
        isPhaseRunning = false;

        float finalPhaseTime = phaseTime;

        SplitData split = splits.Find(s => s.phaseName == phaseName);

        if (split == null)
        {
            split = new SplitData();
            split.phaseName = phaseName;
            splits.Add(split);
        }

        float previousBest = split.bestTime;

        split.lastTime = finalPhaseTime;

        OnSplitCompleted?.Invoke(
            new SplitData
            {
                phaseName = split.phaseName,
                lastTime = finalPhaseTime,
                bestTime = previousBest,
                completed = true
            }
        );

        if (split.bestTime == 0 || finalPhaseTime < split.bestTime)
            split.bestTime = finalPhaseTime;

        split.completed = true;

        tipUI.CalculateTip(finalPhaseTime);
    }

    public float GetTotalTime()
    {
        return totalTime;
    }

    public float GetPhaseTime() //Novo Get pra usar com o Tip
    {
        return phaseTime;
    }

    public void StopRun()
    {
        isRunning = false;
        isPhaseRunning = false;
    }
}