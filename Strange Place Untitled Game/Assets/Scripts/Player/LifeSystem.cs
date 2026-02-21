using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] float maxLifes;

    Animator animator;
    PlayerMovement pm;
    float curLifes;

    void Awake()
    {
        animator = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        curLifes = maxLifes;
    }

    public void SetAlive()
    {
        pm.canMove = true;
        curLifes = maxLifes;
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
        //pm.canMove = false;
        animator.SetTrigger("Die");
        Debug.Log("morri");
    }



}
