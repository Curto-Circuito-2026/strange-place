using UnityEngine;
using DG.Tweening;

public class PhaseSelectorAnimation : MonoBehaviour
{
    public float duraction = 1f;

    public void Awake()
    {
        Vector3 centerScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        transform.DOMove(centerScreen, duraction).SetEase(Ease.OutQuad);
    }
}