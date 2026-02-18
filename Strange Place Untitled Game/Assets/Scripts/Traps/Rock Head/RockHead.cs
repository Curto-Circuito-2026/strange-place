using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

public class RockHead : MonoBehaviour, ITrap
{
    public List<Transform> waypoints; 
    public float speed = 3f;
    public float distance = 0.2f;
    public float delayTime = 1f; 

    public LayerMask layers;
    public float checkDistance = 0.6f;

    private int curWaypoint = 0;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isWaiting = false; 
    private bool blinkedThisLeg = false;

    public bool IsOn { get; set; } = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        if (!isWaiting && IsOn)
        {
            Move();
        }
    }

    void Move()
    {
        if (waypoints.Count == 0) return;

        Transform destin = waypoints[curWaypoint];
        Vector2 newPos = Vector2.MoveTowards(rb.position, destin.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        float distToDestin = Vector2.Distance(rb.position, destin.position);

        if (!blinkedThisLeg && distToDestin < Vector2.Distance(waypoints[(curWaypoint == 0 ? waypoints.Count - 1 : curWaypoint - 1)].position, destin.position) / 2f)
        {
            animator.SetTrigger("Blink");
            blinkedThisLeg = true;
        }

        if (distToDestin < distance)
        {
            StartCoroutine(WaitAtWaypoint(destin.position));
        }
    }

    IEnumerator WaitAtWaypoint(Vector2 reachedPos)
    {
        isWaiting = true; 

        Vector2 direction = (reachedPos - (Vector2)transform.position).normalized;
        TriggerAnimatorByDirection(direction);

        yield return new WaitForSeconds(delayTime);

        curWaypoint = (curWaypoint + 1) % waypoints.Count;
        blinkedThisLeg = false;
        isWaiting = false;
    }

    void TriggerAnimatorByDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) animator.SetTrigger("Right");
            else animator.SetTrigger("Left");
        }
        else
        {
            if (dir.y > 0) animator.SetTrigger("Top");
            else animator.SetTrigger("Bottom");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direcaoEmpurro = -collision.contacts[0].normal;
            RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, direcaoEmpurro, checkDistance, layers);

            if (hit.collider != null)
            {
                Debug.Log("morri");
            }
        }
    }

    public void SetState(bool state)
    {
        IsOn = state;
    }
}