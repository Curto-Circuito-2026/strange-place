using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

public class RockHead : MonoBehaviour
{
    public List<Transform> waypoints; 
    [SerializeField] float speed = 3f;
    [SerializeField] float distance = 0.2f;
    [SerializeField] float delayTime = 1f; 

    [SerializeField] bool hasSpikes = false;
    [SerializeField] LayerMask layers;
    [SerializeField] float checkDistance = 0.6f;

    [SerializeField] int damage = 10;
    private int curWaypoint = 0;
    private Rigidbody2D rb;
    private bool isWaiting = false; 
    private bool blinkedThisLeg = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        if (!isWaiting)
        {
            Move();
        }
    }

    void Move()
    {
        if (waypoints.Count == 0) return;
        if(waypoints[curWaypoint]==null) return;
        Transform destin = waypoints[curWaypoint];
        Vector2 newPos = Vector2.MoveTowards(rb.position, destin.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        float distToDestin = Vector2.Distance(rb.position, destin.position);

        if (!blinkedThisLeg && distToDestin < Vector2.Distance(waypoints[(curWaypoint == 0 ? waypoints.Count - 1 : curWaypoint - 1)].position, destin.position) / 2f)
        {
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
        yield return new WaitForSeconds(delayTime);
        curWaypoint = (curWaypoint + 1) % waypoints.Count;
        blinkedThisLeg = false;
        isWaiting = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasSpikes && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LifeSystem>().GetDamage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!hasSpikes && collision.gameObject.CompareTag("Player"))
        {
            Vector2 direcaoEmpurro = -collision.contacts[0].normal;
            RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, direcaoEmpurro, checkDistance, layers);

            if (hit.collider != null)
            {
                collision.gameObject.GetComponent<LifeSystem>().GetDamage(damage);
            }
        }
    }
}