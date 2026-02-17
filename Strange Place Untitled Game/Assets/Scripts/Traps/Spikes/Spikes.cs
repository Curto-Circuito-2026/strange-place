using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour, ITrap
{
    public bool IsOn { get; set; } = true;

    public void SetState(bool state)
    {
        IsOn = state;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("matei");
        }
    }
}
