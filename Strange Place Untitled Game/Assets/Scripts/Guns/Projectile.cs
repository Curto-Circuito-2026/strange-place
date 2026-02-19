using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configurações Visuais")]
    [SerializeField] Sprite sprite;

    [Header("Atributos")]
    [SerializeField] float speed = 10f;
    [SerializeField] float damage = 10f;
    [SerializeField] float lifeTime = 5f;

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
        Debug.Log("Colidiu com " + collision.name);
        Destroy(gameObject);
    }
}