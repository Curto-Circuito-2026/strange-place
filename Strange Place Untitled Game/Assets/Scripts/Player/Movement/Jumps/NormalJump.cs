using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class NormalJump : MonoBehaviour, IJump
{
    public bool CanJump { get; private set; } = true;
    public Animator Animator { get; set; }

    //todo
    //dar um jeito de tirar esses valores setados por codigo e passar pro inspetor

    #region jump
    [Header("Força do pulo")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float wallReflectForce = 50f;
    
    [Header("Camadas de detecção")]
    [SerializeField] LayerMask groundLayer= (1<<8);
    [SerializeField] LayerMask wallLayer = (1<<9);

    float opositeForce;

    #endregion


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