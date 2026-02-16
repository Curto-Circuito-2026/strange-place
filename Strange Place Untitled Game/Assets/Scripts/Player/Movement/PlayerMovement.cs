using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject initialRunPrefab;

    public bool canRun = true;
    public bool canJump = true;
    IRun curRun;
    IJump curJump;
    Rigidbody2D rb;
    Vector2 _input;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRun(initialRunPrefab);
    }

    void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && canJump && curJump !=null)
        {
            curJump.Jump(rb);
        }
    }

    private void FixedUpdate()
    {
        if(canRun && curRun !=null) curRun.Move(rb,_input);
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
    }
}
}