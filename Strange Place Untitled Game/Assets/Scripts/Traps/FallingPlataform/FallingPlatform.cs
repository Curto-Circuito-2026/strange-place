using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatform : MonoBehaviour, ITrap
{
    [SerializeField] float fallDelay;
    [SerializeField] float destroyDelay;
    Animator animator;
    Rigidbody2D rb;

    BoxCollider2D boxCollider;
    public bool IsOn { get; set; } = true;

    private void Awake() 
    {
        animator = GetComponent<Animator>();    
        boxCollider = GetComponent<BoxCollider2D>();    
        rb = GetComponent<Rigidbody2D>();   
    }
    public void SetState(bool state)
    {   
        IsOn = state;
        animator.enabled = IsOn;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        SetState(false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        boxCollider.enabled = false;
        Destroy(gameObject,destroyDelay);

    }
}
