using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireTrap : MonoBehaviour, ITrap
{

    [SerializeField] int damage = 10;
    [SerializeField] float timeUntilFire;
    [SerializeField] float fireOnTime;
    public bool IsOn { get; set; }

    bool burning;

    Animator animator;
    CircleCollider2D fireCollider;
    private void Awake() 
    {
        animator = GetComponent<Animator>();
        fireCollider = GetComponent<CircleCollider2D>();
    }
    public void SetState(bool state)
    {
        IsOn = state;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && burning)
        {
            collision.gameObject.GetComponent<LifeSystem>().GetDamage(damage);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !burning)
        {
            animator.SetTrigger("Hit");
            StartCoroutine(StartBurning());
        }
    }

    IEnumerator StartBurning()
    {
        burning = true;
        yield return new WaitForSeconds(timeUntilFire);
        animator.SetBool("Burning",burning);
        fireCollider.enabled = true;
        yield return new WaitForSeconds(fireOnTime);
        burning = false;
        animator.SetBool("Burning",burning);
        fireCollider.enabled = burning;
    }

}
