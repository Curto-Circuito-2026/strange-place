using System;
using UnityEngine;

public class FirstEnemyTest : EnemyBase
{
    [SerializeField] bool isShooter;
    [SerializeField] float shootTime;

    [SerializeField] Projectile projectile;

    [SerializeField] bool shootToRight;

    [SerializeField] float xOffset = 1f; 

    float curTime;

    public override void OnDeath()
    {
        Debug.Log("MORRI");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.otherCollider is CapsuleCollider2D)
            {
                collision.gameObject.GetComponent<LifeSystem>().GetDamage(1000); 
            }
            else if (collision.otherCollider is BoxCollider2D)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (isShooter)
        {
            curTime += Time.deltaTime;

            if (curTime >= shootTime)
            {
                curTime = 0;
                animator.SetTrigger("Shoot");
                Vector2 direction = shootToRight ? Vector2.right : Vector2.left;

                Vector3 spawnPosition = transform.position + 
                                        new Vector3(shootToRight ? xOffset : -xOffset, 0f, 0f);

                Projectile newProjectile = Instantiate(projectile, spawnPosition, Quaternion.identity);
                newProjectile.SetDirection(direction);
            }
        }
    }
}