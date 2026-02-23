using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class NormalJump : MonoBehaviour, IJump
{
    public bool CanJump { get; private set; } = true;
    public Animator Animator { get; set; }

    public float jumpForce { get; set; }
    public float wallReflectForce { get; set; }

    bool inWall;

    float opositeForce;

    public void Jump(Rigidbody2D rb)
    {

        Animator.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(wallReflectForce*opositeForce, jumpForce);
        CanJump = false;
        Animator.SetBool("isGrounded",false);
        Animator.SetBool("inWall",false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Vector2 normal = collision.contacts[0].normal;


        if (1f - Mathf.Abs(normal.x) <= 0.1f ) //wall
        {
            Animator.SetBool("inWall", true);
            CanJump = true;
            opositeForce = normal.x;

        }

        else if (1f - Mathf.Abs(normal.y) <= 0.1f) // ground
        {
            Animator.SetBool("isGrounded", true);
            Animator.SetBool("inWall", false);
            CanJump = true;
            opositeForce = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Animator.SetBool("inWall", false);
        
    }
}