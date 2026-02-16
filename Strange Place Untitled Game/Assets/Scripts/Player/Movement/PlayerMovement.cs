using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject initialRunPrefab;
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
        if (value.isPressed)
        {
            curJump.Jump(rb);
        }
    }

    private void FixedUpdate()
    {
        curRun.Move(rb,_input);
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
            System.Type tipoDoScript = prefabClass.GetType();
            curJump = gameObject.AddComponent(tipoDoScript) as IJump;
            Debug.Log(tipoDoScript.Name);
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
        System.Type tipoDoScript = prefabClass.GetType();
        curRun = gameObject.AddComponent(tipoDoScript) as IRun;
        Debug.Log(tipoDoScript.Name);
    }
}
}