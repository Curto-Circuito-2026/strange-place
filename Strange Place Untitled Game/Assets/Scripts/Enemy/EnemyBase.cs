using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Configurações de Base")]

    [SerializeField] protected bool CanMove = true;
    [SerializeField] protected AnimatorOverrideController animatorOverride; // Pra trocar as animações sem criar um novo Controller
    [SerializeField] protected List<Transform> waypoints;
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float waypointDistance = 0.5f; // Distancia para contar que chegou no waypoint
    [SerializeField] protected float waypointWaitTime = 0.1f; // tempo que vai esperar em cima do waypoint
    [SerializeField] protected bool isReverse = false; // Se true ele vai seguir o array de waypoints nessa ordem: 1-2-3-2-1, se false faz 1-2-3-1-2-3

    [Header("Detecção e Pulo")]
    [SerializeField] protected LayerMask obstacleLayer; // O que ele considera chão ou parede
    [SerializeField] protected float rayDistance = 0.7f; // Distância do raio que checka paredes
    [SerializeField] protected float jumpForce = 12;
    [SerializeField] protected float lowJumpMultiplier = 0.6f; // quanto deve reduzir o pulo para waypoints abaixo do personagem

    [SerializeField] protected Vector2 rayOffset = new Vector2(0, 0.2f); // offsett do raycast que ve se tem parede na frente
    
    [Header("Tolerância de Altura (Y)")]
    // evitam que ele pule se o próximo waypoint estiver quase na mesma altura
    [SerializeField] protected float yUpThreshold = 0.5f;   // offset pra cima do personagem
    [SerializeField] protected float yDownThreshold = 2; // offset pra baixo do personagem

    [Header("Sensores de Chão e Buraco")]
    [SerializeField] protected float groundRayDistance = 0.3f; // tamanho do ray de grounded
    [SerializeField] protected Vector2 groundRayOffset = new Vector2(0, -0.6f); // offset do raycast de grounded em relacao ao meio do personagem
    [SerializeField] protected float gapCheckDistance = 0.4f; // O quão pra frente ele olha pra ver se tem buraco

    [Header("Perseguição (Opcional)")]
    [SerializeField] protected bool followPlayer = false; // se true ele vai atras do player quando o player estiver dentro do rage, se false ele so vai ficar nos waypoints
    [SerializeField] protected float detectionRange = 5; // distancia de deteccao do player
    [SerializeField] protected Transform playerTransform;

    protected int curWaypoint = 0;
    protected int patrolDirection = 1;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isGrounded;
    protected float verticalVelocity;
    protected bool canJump = true;
    protected bool isJumping = false;
    protected bool isWaitingAtWaypoint = false;

    Vector2 originalPos;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
        
        if (followPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
        originalPos = transform.position;
    }

    public virtual void Reset()
    {
        transform.position = originalPos;
    }
    protected virtual void FixedUpdate()
    {
        CheckGround(); 
        
        if(!CanMove) return;

        if (isWaitingAtWaypoint) 
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        Vector2 targetPos = GetCurrentTarget();
        verticalVelocity = rb.linearVelocity.y;

        if (!isWaitingAtWaypoint) CheckObstacles(targetPos);

        Move(targetPos);
    }

    // Decide se o alvo é o player ou o próximo waypoint da patrulha
    private Vector2 GetCurrentTarget()
    {
        if (followPlayer && playerTransform != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distToPlayer <= detectionRange) return playerTransform.position;
        }

        if (waypoints != null && waypoints.Count > 0 && waypoints[curWaypoint] != null)
        {
            float distToWaypoint = Vector2.Distance(transform.position, waypoints[curWaypoint].position);
            
            if (distToWaypoint <= waypointDistance)
            {
                if (!isWaitingAtWaypoint) StartCoroutine(WaitAtWaypointRoutine());
            }
            return waypoints[curWaypoint].position;
        }
        return transform.position;
    }

    IEnumerator WaitAtWaypointRoutine()
    {
        isWaitingAtWaypoint = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
        UpdateWaypointIndex(); 
        yield return new WaitForSeconds(waypointWaitTime);
        isWaitingAtWaypoint = false;
    }


    private void UpdateWaypointIndex()
    {
        if (waypoints.Count <= 1) return;
        
        if (isReverse) 
        {
            curWaypoint += patrolDirection;
            if (curWaypoint >= waypoints.Count) { patrolDirection = -1; curWaypoint = waypoints.Count - 2; }
            else if (curWaypoint < 0) { patrolDirection = 1; curWaypoint = 1; }
        }
        else 
        {
            curWaypoint = (curWaypoint + 1) % waypoints.Count;
        }
        
        curWaypoint = Mathf.Clamp(curWaypoint, 0, waypoints.Count - 1);
    }

    protected virtual void Move(Vector2 target)
    {
        float diffX = target.x - transform.position.x;
        
        // offsett pra ele na o precisar estar exatamente em cima do waypoint
        if (Mathf.Abs(diffX) > 0.05f) 
        {
            float direction = diffX > 0 ? 1 : -1;
            float currentSpeed = isJumping ? speed * 0.8f : speed; 
            
            rb.linearVelocity = new Vector2(direction * currentSpeed, verticalVelocity);
            
            if (Mathf.Abs(diffX) > waypointDistance)
                transform.localRotation = direction > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            
        }
        else
        {
            rb.linearVelocity = new Vector2(0, verticalVelocity);
        }
    }

    protected virtual void CheckObstacles(Vector2 target)
    {
        float distToTarget = Vector2.Distance(transform.position, target);
        
        if (!canJump || !isGrounded || distToTarget < waypointDistance) return;

        float yDiff = target.y - transform.position.y;

        if (yDiff < 0 && Mathf.Abs(yDiff) <= yDownThreshold) return;
        if (yDiff > 0 && yDiff <= yUpThreshold) return;

        Vector2 dirToTarget = (target - (Vector2)transform.position).normalized;
        float horizontalDir = dirToTarget.x > 0 ? 1 : -1;
        
        Vector2 wallOrigin = (Vector2)transform.position + rayOffset;
        RaycastHit2D hitWall = Physics2D.Raycast(wallOrigin, Vector2.right * horizontalDir, rayDistance, obstacleLayer);

        Vector2 gapOrigin = (Vector2)transform.position + new Vector2(horizontalDir * gapCheckDistance, groundRayOffset.y);
        RaycastHit2D hitGap = Physics2D.Raycast(gapOrigin, Vector2.down, groundRayDistance + 0.1f, obstacleLayer);

        if (hitWall.collider != null || hitGap.collider == null)
        {
            float finalJumpForce = jumpForce;
            if (yDiff < -0.5f) finalJumpForce *= lowJumpMultiplier; 
            StartCoroutine(JumpRoutine(finalJumpForce));
        }
    }

    IEnumerator JumpRoutine(float force)
    {
        canJump = false;
        isJumping = true;
        verticalVelocity = force;
        yield return new WaitForSeconds(0.1f);
        isJumping = false;

        yield return new WaitUntil(() => isGrounded); 
        yield return new WaitForSeconds(0.1f); 
        canJump = true;
    }

    private void CheckGround()
    {
        Vector2 origin = (Vector2)transform.position + groundRayOffset;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundRayDistance, obstacleLayer);
        isGrounded = hit.collider != null;
    }

    // cada inimigo tem seu proprio metodo de morte;
    public abstract void OnDeath();

    private void OnDrawGizmos()
    {
        // circulo de deteccao do player
        if (followPlayer)
        {
            Transform tempPlayer = playerTransform;
            if (tempPlayer == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) tempPlayer = p.transform;
            }

            if (tempPlayer != null)
            {
                float dist = Vector2.Distance(transform.position, tempPlayer.position);
                Gizmos.color = dist <= detectionRange ? Color.green : Color.red;
                Gizmos.DrawWireSphere(transform.position, detectionRange);
            }
        }

        // Raio de deteccao da parede 
        Vector2 target = GetCurrentTarget();
        float horizontalDir = (target.x - transform.position.x) > 0 ? 1 : -1;
        Vector2 wallOrigin = (Vector2)transform.position + rayOffset;
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallOrigin, Vector2.right * horizontalDir * rayDistance);
        // -----------------------

        // offset para o ver se ele precisa pular para pegar o waypoint
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + new Vector3(0, (yUpThreshold - yDownThreshold) / 2, 0);
        Vector3 size = new Vector3(waypointDistance * 4, yUpThreshold + yDownThreshold, 0);
        Gizmos.DrawWireCube(center, size);

        // raycast na frente pra ver se tem chao ou ele deve pular
        if (waypoints != null && waypoints.Count > 0)
        {
            Vector2 gapOrigin = (Vector2)transform.position + new Vector2(horizontalDir * gapCheckDistance, groundRayOffset.y);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(gapOrigin, Vector2.down * (groundRayDistance + 0.1f));
        }
    }
}