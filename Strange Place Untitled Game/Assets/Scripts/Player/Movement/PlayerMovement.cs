using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject initialRunPrefab;
    [SerializeField] GameObject initialJumpPrefab;

    #region run
    [Header("Velocidade do movimento")]
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float deceleration = 12f;
    #endregion

    #region jump
    [Header("Força do pulo")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float wallReflectForce = 50f;

    [Header("Camadas de detecção")]
    [SerializeField] LayerMask groundLayer = (1 << 8);
    [SerializeField] LayerMask wallLayer = (1 << 9);
    #endregion


    public bool canRun = true;
    public bool canJump = true;
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
        if(value.isPressed && !curRun.IsSliding && curRun != null)
        {
            slideCoroutine = StartCoroutine(curRun.SlideCor(rb, _input));
            UnityEngine.Debug.Log("haa");
        }
    }


    void StopSlide()
    {
        curRun.IsSliding = false;
        animator.SetBool("isGrounded", true);
        animator.SetBool("isSliding", false);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        StopCoroutine(slideCoroutine);
    }


    public void OnJump(InputValue value)
    {
        if (value.isPressed && canJump && curJump !=null)
        {
            curJump.Jump(rb);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            if (curRun.IsSliding)
            {
                StopSlide();
            }
        }
    }

    private void FixedUpdate()
    {
        if(canRun && curRun !=null && !curRun.IsSliding) 
        {
            animator.SetBool("isRunning", _input.x != 0);
            curRun.Move(rb,_input);
            if (_input.x < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        };
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
            curJump.SetValues(jumpForce, wallReflectForce, groundLayer, wallLayer);
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
        curRun.SetValues(maxSpeed, acceleration,deceleration);
        curRun.Animator = animator;
    }
}
}