using UnityEditor;
using UnityEngine;

public class Saw : MonoBehaviour, ITrap
{
    [SerializeField] float damage;
   
    public bool IsOn {get;set;}

    [Header("Movimento por waypoints")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed;
    Animator animator;
    int currentWaypointIndex = 0;


    void Awake()
    {
        animator = GetComponent<Animator>();
        SetState(IsOn);
    }

    void Update()
    {
        Move();
        //SÓ PRA TESTAR NO INSPETOR
        //ChangeState(IsOn);
    }

    void Move()
    {
        if(waypoints.Length == 0 || !IsOn) return;
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
 
    }
    public void SetState(bool state)
    {   
        IsOn = state;
        animator.enabled = IsOn;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(IsOn && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("matei");
        }

    }
}
