using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidade do movimento")]
    [SerializeField] float maxSpeed = 12f;      
    [SerializeField] float acceleration = 10f;  
    [SerializeField] float deceleration = 12f;  

    Rigidbody2D RB;
    float _move;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>().x;
    }

    private void FixedUpdate()
    {
        float targetSpeed = _move * maxSpeed;
        float speedDif = targetSpeed - RB.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        RB.AddForce(Vector2.right * movement, ForceMode2D.Force);
    }
}