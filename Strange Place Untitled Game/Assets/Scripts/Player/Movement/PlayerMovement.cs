using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject initialRunPrefab;
    [SerializeField] GameObject initialJumpPrefab;

    public bool canRun = true;
    public bool canJump = true;
    IRun curRun;
    IJump curJump;

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

    public void OnJump(InputValue value)
    {
        if (value.isPressed && canJump && curJump.CanJump && curJump !=null)
        {
            curJump.Jump(rb);
        }
    }

    private void FixedUpdate()
    {
        if(canRun && curRun !=null) 
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
        curRun.Animator = animator;
    }
}
}