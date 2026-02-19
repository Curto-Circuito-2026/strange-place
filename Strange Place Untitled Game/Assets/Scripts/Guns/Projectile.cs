using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] float speed = 10;
    [SerializeField] int damage = 10;
    [SerializeField] float lifeTime = 5;

    private Vector2 moveDirection; 
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(sprite!=null)
        spriteRenderer.sprite = sprite;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; 
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LifeSystem>().GetDamage(damage);    
        }
        
        Destroy(gameObject);
    }
}