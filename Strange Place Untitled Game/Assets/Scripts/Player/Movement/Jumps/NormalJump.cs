using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class NormalJump : MonoBehaviour, IJump
{
    public bool CanJump { get; private set; }


    //todo

    //dar um jeito de tirar esses valores setados por codigo e passar pro inspetor
    #region jump
    [Header("Força do pulo")]
    [SerializeField] float jumpForce = 14f;
    [SerializeField] float wallReflectForce = 50f;
    
    [Header("Camadas de detecção")]
    [SerializeField] LayerMask groundLayer= (1<<8);
    [SerializeField] LayerMask wallLayer = (1<<9);

    
    bool canJump = true;
    float opositeForce;

    #endregion


    public void Jump(Rigidbody2D rb)
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