using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] float maxLifes = 1;

    Animator animator;
    PlayerMovement pm;
    [SerializeField] float curLifes;

    CapsuleCollider2D playerCollider;
    Vector2 originalColliderSize;

    void Awake()
    {
        animator = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        originalColliderSize = playerCollider.size;
    }
    void Start()
    {
        curLifes = maxLifes;
    }

    void Update()
    {
        if(curLifes>0)
        {
            SetAlive();
        }
    }
    public void SetAlive()
    {
        pm.canMove = true;
        curLifes = maxLifes;
        playerCollider.size = originalColliderSize;
    }

    public void GetDamage(int damage)
    {
        curLifes -= damage;
        if(curLifes<=0)
        {
            Die();
        }
    }

    void Die()
    {
        
        animator.SetBool("Dead",true);
        pm.canMove = false;
        playerCollider.size = new Vector2(0.01f,0.000f);
    }



}
