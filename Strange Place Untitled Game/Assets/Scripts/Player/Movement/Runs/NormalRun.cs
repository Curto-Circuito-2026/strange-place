

using UnityEngine;
using System.Collections;

public class NormalRun : MonoBehaviour, IRun
{
    public float maxSpeed { get; set; }
    public float acceleration { get; set; }
    public float deceleration { get; set; }

    public bool IsSliding { get; set; }
    public Animator Animator { get; set; }

    public void Move(Rigidbody2D rb,Vector2 input)
    {
        float targetSpeed = input.x * maxSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);
    }

    public IEnumerator SlideCor(Rigidbody2D rb, Vector2 input)
    {
        Animator.SetBool("isSliding", true);
        IsSliding = true;
        float targetSpeed = input.x * maxSpeed * 1.5f;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
        yield return new WaitForSeconds(2f);

        rb.linearVelocity = new Vector2(targetSpeed * 0.5f, rb.linearVelocity.y);

        IsSliding = false;
        Animator.SetBool("isSliding", false);


    }


}