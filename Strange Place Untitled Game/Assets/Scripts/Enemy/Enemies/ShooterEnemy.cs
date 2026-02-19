using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float timeUntilShoot = 2f;

    [SerializeField] float shootOffsetX = 1f;
    [SerializeField] float shootOffsetY = 0f;

    [SerializeField] Vector2 shootDirection = Vector2.right;

    private float timeCounter;

    void Update()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= timeUntilShoot)
        {
            timeCounter = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 spawnPosition = transform.position + new Vector3(shootOffsetX, shootOffsetY, 0);
        Projectile newProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        newProjectile.SetDirection(shootDirection);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 spawnPosition = transform.position + new Vector3(shootOffsetX, shootOffsetY, 0);
        Gizmos.DrawWireSphere(spawnPosition, 0.2f);
        Gizmos.DrawRay(spawnPosition, (Vector3)shootDirection * 1.5f);
    }

    public override void OnDeath()
    {
        Debug.Log("morr!");
        Destroy(gameObject);
    }
}