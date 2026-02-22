using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour, ITrap
{
    [SerializeField] float fallDelay;
    [SerializeField] float destroyDelay;

    [SerializeField] ParticleSystem particles;

    [SerializeField] float decreaseHeight;
    Animator animator;
    Rigidbody2D rb;

    Vector2 originalPos;
    RigidbodyType2D originalRbType;

    BoxCollider2D boxCollider;
    public bool IsOn { get; set; } = true;

    private void Awake() 
    {
        animator = GetComponent<Animator>();    
        boxCollider = GetComponent<BoxCollider2D>();    
        rb = GetComponent<Rigidbody2D>();   
        originalPos = transform.position;
        originalRbType = rb.bodyType;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y-decreaseHeight);
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        SetState(false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        boxCollider.enabled = false;
        //Destroy(gameObject,destroyDelay);

    }

    public void Reset()
    {
        boxCollider.enabled = true;
        rb.bodyType = originalRbType;
        transform.position = originalPos;
    }
}
