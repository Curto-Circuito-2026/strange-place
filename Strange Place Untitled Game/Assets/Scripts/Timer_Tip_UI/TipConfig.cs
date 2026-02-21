using UnityEngine;

[CreateAssetMenu(fileName = "TipConfig", menuName = "Game/Tip Config")]
public class TipConfig : ScriptableObject
{
    [Header("Tempo (em segundos)")]

    //Testar tempos depois
    public float maxTime = 60f;      // < 1 min
    public float highTime = 120f;    // 1-2 min
    public float mediumTime = 180f;  // 2-3 min
    public float lowTime = 240f;     // 3-4 min

    [Header("Valores de Gorjeta")]

    //Confirmar valores das gorjetas
    public int maxTip = 100;
    public int highTip = 75;
    public int mediumTip = 50;
    public int lowTip = 25;
    public int noTip = 0;
}