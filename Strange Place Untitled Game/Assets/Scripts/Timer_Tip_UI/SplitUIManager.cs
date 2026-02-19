using UnityEngine;

public class SplitUIManager : MonoBehaviour
{
    public Transform splitsContainer;
    public GameObject splitItemPrefab;

    void Start()
    {
        GameRunTimer.Instance.OnSplitCompleted += CreateSplitUI;
    }

    void CreateSplitUI(SplitData split)
    {
        if (splitItemPrefab == null)
            Debug.LogError("SplitItemPrefab está NULL!");

        if (splitsContainer == null)
            Debug.LogError("SplitsContainer está NULL!");

        GameObject obj = Instantiate(splitItemPrefab, splitsContainer);

        SplitItemUI item = obj.GetComponent<SplitItemUI>();

        item.Setup(split.phaseName, split.lastTime, split.bestTime);
    }
}
