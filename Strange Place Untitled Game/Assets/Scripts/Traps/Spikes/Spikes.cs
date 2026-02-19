using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour, ITrap
{
    [SerializeField] int damage = 10;
    public bool IsOn { get; set; } = true;

    public void SetState(bool state)
    {
        IsOn = state;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LifeSystem>().GetDamage(damage);
        }
    }
}
