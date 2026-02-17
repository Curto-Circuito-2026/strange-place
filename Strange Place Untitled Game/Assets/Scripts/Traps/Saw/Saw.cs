using UnityEditor;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] float damage;
   
    [SerializeField] bool isOn;

    [Header("Movimento por waypoints")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed;
    Animator animator;
    int currentWaypointIndex = 0;
    void Awake()
    {
        animator = GetComponent<Animator>();
        ChangeState(isOn);
    }

    void Update()
    {
        Move();
        //SÓ PRA TESTAR NO INSPETOR
        //ChangeState(isOn);
    }

    void Move()
    {
        if(waypoints.Length == 0 || !isOn) return;
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
 
    }
    public void ChangeState(bool state)
    {   
        isOn = state;
        animator.enabled = isOn;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isOn && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("matei");
        }

    }

}
