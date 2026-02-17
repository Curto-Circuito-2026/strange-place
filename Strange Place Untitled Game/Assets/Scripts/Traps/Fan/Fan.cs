using Unity.VisualScripting;
using UnityEngine;

public class Fan : MonoBehaviour, ITrap
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] float fanForce;
    public bool IsOn { get; set; } = true;

    Animator animator;

    BoxCollider2D boxCollider;

    

    void Awake()
    {
        animator = GetComponent<Animator>();        
        boxCollider = GetComponent<BoxCollider2D>();        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * fanForce, ForceMode2D.Force);
        }
    }
    public void SetState(bool state)
    {
        IsOn = state;
        animator.enabled = IsOn;
        if(IsOn)
        {
            particles.Play();
        }
        else
        {
            particles.Pause();
        }
    }


}
