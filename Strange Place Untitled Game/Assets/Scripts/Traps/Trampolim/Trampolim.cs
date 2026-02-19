using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Trampolim : MonoBehaviour, ITrap
{
    [SerializeField] float jumpForce;
    Animator animator;

    public bool IsOn { get; set; } = true;

    public void SetState(bool state)
    {
        IsOn = state;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && IsOn)
        {
            animator.SetTrigger("Jump");
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForceY(jumpForce,ForceMode2D.Impulse);
        }
    }
}
