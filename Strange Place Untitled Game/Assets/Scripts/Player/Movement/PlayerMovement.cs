using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject initialRunPrefab;
    [SerializeField] GameObject initialJumpPrefab;

    [SerializeField] CapsuleCollider2D playerCollider;

    #region run
    [Header("Velocidade do movimento")]
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float deceleration = 12f;
    [SerializeField] float slideThreshold = 5f;
    #endregion

    #region jump
    [Header("For�a do pulo")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float wallReflectForce = 50f;
    #endregion

    public bool canMove = true;
    IRun curRun;
    IJump curJump;

    Coroutine slideCoroutine { get; set; }

    Animator animator;
    Rigidbody2D rb;
    Vector2 _input;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        SetRun(initialRunPrefab);
        SetJump(initialJumpPrefab);
    }

    void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
    }

    void OnCrouch(InputValue value)
    {
        if(curRun.IsSliding || curRun == null || !curJump.CanJump)
        {
            return;
        }

        if (value.isPressed)
        {
            if (Mathf.Abs(rb.linearVelocity.x) >= slideThreshold && canMove)
            {
                curRun.StartSlide(rb, _input);
            }
            else
            {
                curRun.ForcedCrouch = false;
                curRun.Crouch();
            }

        }

        else if (curRun.IsCrouching)
        {
            curRun.UnCrouch();
        }
    }


    public void OnJump(InputValue value)
    {
        if (value.isPressed && curJump.CanJump && curJump !=null && canMove)
        {
            curJump.Jump(rb);
        }
    }
     
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        if (1f - Mathf.Abs(normal.x) <= 0.1f) // wall
        {
            if (curRun.IsSliding)
            {curRun.StopSlide(rb);}

        }

    }



    private void FixedUpdate()
    {
        if (curRun == null)
        {
            return;
        }
        if (!curRun.IsSliding && canMove)
        {
            animator.SetBool("isRunning", _input.x != 0);
            curRun.Move(rb, _input);
            if (_input.x < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        };

        if (curRun.ForcedCrouch)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.up, 1f, ~LayerMask.GetMask("Player"));
            if (hit.collider == null) { curRun.UnCrouch(); }
        }
    }

    public void SetJump(GameObject jumpPrefab) 
    {
        if (curJump != null) 
        {
            Destroy(curJump as MonoBehaviour);
        }

        IJump prefabClass = jumpPrefab.GetComponent<IJump>();

        if (prefabClass != null) 
        {

            System.Type scriptType = prefabClass.GetType();
            curJump = gameObject.AddComponent(scriptType) as IJump;
            curJump.SetValues(jumpForce, wallReflectForce);
            curJump.Animator = animator;
        }
    }

    public void SetRun(GameObject runPrefab) 
{
    if (curRun != null) 
    {
        Destroy(curRun as MonoBehaviour);
    }

    IRun prefabClass = runPrefab.GetComponent<IRun>();

    if (prefabClass != null) 
    {
        System.Type scriptType = prefabClass.GetType();
        curRun = gameObject.AddComponent(scriptType) as IRun;
        curRun.SetValues(maxSpeed, acceleration,deceleration, playerCollider);
        curRun.Animator = animator;
    }
}
}