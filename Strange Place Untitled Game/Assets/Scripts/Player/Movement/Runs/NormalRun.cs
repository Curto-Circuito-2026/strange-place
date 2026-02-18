using UnityEngine;
using System.Collections;
 public class NormalRun : MonoBehaviour, IRun
{
    public float maxSpeed { get; set; }
    public float acceleration { get; set; }
    public float deceleration { get; set; }

    public bool IsSliding { get; set; }
    public bool IsCrouching { get; set; }
    public bool ForcedCrouch { get; set; }

    public Animator Animator { get; set; }

    public CapsuleCollider2D playerCollider { get; set; }

    public Coroutine slideCoroutine { get; set; }

    public void Move(Rigidbody2D rb,Vector2 input)
    {
        float targetSpeed = input.x * maxSpeed;
        if (IsCrouching) targetSpeed *= 0.5f;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);
    }

    public IEnumerator SlideCor(Rigidbody2D rb, Vector2 input)
    {
        Animator.SetBool("isSliding", true);
        playerCollider.offset = new Vector2(0, -0.05f);
        playerCollider.size = new Vector2(0.15f, 0.0001f);
        IsSliding = true;
        float targetSpeed = input.x * maxSpeed * 1.5f;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
        yield return new WaitForSeconds(1f);
        rb.linearVelocity = new Vector2(targetSpeed * 0.5f, rb.linearVelocity.y);
        yield return new WaitForSeconds(0.2f);
        StopSlide(rb);
        
    } 

    public void StartSlide(Rigidbody2D rb, Vector2 input)
    {
        slideCoroutine = StartCoroutine(SlideCor(rb, input));
    }

    public void StopSlide(Rigidbody2D rb)
    {
        IsSliding = false;
        Animator.SetBool("isGrounded", true);
        Animator.SetBool("isSliding", false);
        StopCoroutine(slideCoroutine);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        

        RaycastHit2D hit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.up, 1f, ~LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            ForcedCrouch = true;
            Crouch();
        }
        else
        {
            UnCrouch();
        }

    }

    public void Crouch()
    {
        Animator.SetBool("isCrouching", true);
        IsCrouching = true;
        playerCollider.offset = new Vector2(0, -0.05f);
        playerCollider.size = new Vector2(0.15f, 0.0001f);
    }
    public void UnCrouch()
    {
        Animator.SetBool("isCrouching", false);
        IsCrouching = false;
        ForcedCrouch = false;
        playerCollider.offset = new Vector2(0, 0);
        playerCollider.size = new Vector2(0.2f, 0.25f);
    }
     

}