

using UnityEngine;

public class InvertedRun : MonoBehaviour, IRun
{

    //todo

    //dar um jeito de tirar esses valores setados por codigo e passar pro inspetor

    #region run
    [Header("Velocidade do movimento")]
    [SerializeField] float maxSpeed = 12f;      
    [SerializeField] float acceleration = 10f;  
    [SerializeField] float deceleration = 12f;  

    #endregion
    public void Move(Rigidbody2D rb,Vector2 input)
    {
        float targetSpeed = input.x * -1 * maxSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);
    }
}