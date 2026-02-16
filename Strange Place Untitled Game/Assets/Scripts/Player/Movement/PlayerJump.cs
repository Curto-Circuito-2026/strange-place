using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    [Header("Força do pulo")]
    [SerializeField] float jumpForce = 14f;
    [SerializeField] float wallReflectForce = 50f;
    
    [Header("Camadas de detecção")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    Rigidbody2D rb;
    bool canJump = true;
    float opositeForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && canJump)
        {
            JumpRoutine();
        }
    }

    void JumpRoutine()
    {
        rb.linearVelocity = new Vector2(wallReflectForce*opositeForce, jumpForce);
        canJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            canJump = true;
            opositeForce = 0;
        }
        else if(((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            canJump = true;
            opositeForce = transform.position.x - collision.transform.position.x>0? 1 : -1;
        }
    }
}