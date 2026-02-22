using UnityEngine;
using System.Collections;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] float maxLifes = 1;

    Animator animator;
    Transform transformComp;
    PlayerMovement pm;
    [SerializeField] float curLifes;

    CapsuleCollider2D playerCollider;
    Vector2 originalColliderSize;
    Vector2 originalPosition;

    void Awake()
    {
        transformComp = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        originalColliderSize = playerCollider.size;
        originalPosition = transformComp.position;
    }
    void Start()
    {
        curLifes = maxLifes;
    }

    // void Update()
    // {
    //     if(curLifes>0)
    //     {
    //         SetAlive();
    //     }
    // }
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

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        transformComp.position = originalPosition;
        SetAlive();
        ResetFallingPlatform();
        animator.SetBool("Dead",false);
        animator.SetTrigger("Revive");
    }


    void ResetFallingPlatform()
    {
        
        foreach(FallingPlatform platform in FindObjectsByType<FallingPlatform>(FindObjectsSortMode.None))
        {
            platform.Reset();
        }
    }

    void Die()
    {
        
        animator.SetBool("Dead",true);
        pm.canMove = false;
        playerCollider.size = new Vector2(0.01f,0.000f);

        StartCoroutine(Respawn());
       
    }



}
