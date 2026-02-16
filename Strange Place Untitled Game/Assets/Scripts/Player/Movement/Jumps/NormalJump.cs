using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Diagnostics;


public class NormalJump : MonoBehaviour, IJump
{
    public bool CanJump { get; private set; } = true;
    public Animator Animator { get; set; }

    public float jumpForce { get; set; }
    public float wallReflectForce { get; set; }

    public LayerMask groundLayer { get; set; }

    public LayerMask wallLayer { get; set; }

    float opositeForce;

    public void Jump(Rigidbody2D rb)
    {
        UnityEngine.Debug.Log(jumpForce);

        Animator.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(wallReflectForce*opositeForce, jumpForce);
        CanJump = false;
        Animator.SetBool("isGrounded",false);
        Animator.SetBool("inWall",false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Animator.SetBool("isGrounded",true);
            CanJump = true;
            opositeForce = 0;
        }
        else if(((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Animator.SetBool("inWall",true);
            CanJump = true;
            opositeForce = transform.position.x - collision.transform.position.x>0? 1 : -1;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
       if(((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Animator.SetBool("inWall",false);
        } 
    }
}