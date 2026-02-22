using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Saw : MonoBehaviour, ITrap
{
    [SerializeField] int damage = 10;
    [SerializeField] float velocidadeRotacao = 100f;
    public bool IsOn {get;set;} = true;

    [Header("Movimento por waypoints")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed;
    int currentWaypointIndex = 0;


    void Awake()
    {
        SetState(IsOn);
    }

    void Update()
    {
        Move();
        if(IsOn)
        {
            transform.Rotate(0, 0, -velocidadeRotacao * Time.deltaTime);
        }
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(IsOn && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LifeSystem>().GetDamage(damage);
        }
    }
}
