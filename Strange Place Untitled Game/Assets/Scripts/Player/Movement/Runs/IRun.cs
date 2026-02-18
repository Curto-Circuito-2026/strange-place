using UnityEngine;
using System.Collections;

public interface IRun
{
    float maxSpeed { get; set; }
    float acceleration { get; set; }
    float deceleration { get; set; }

    CapsuleCollider2D playerCollider { get; set; }

    public void SetValues(float _maxSpeed, float _acceleration, float _deceleration, CapsuleCollider2D _collider)
    {
        acceleration = _acceleration;
        deceleration = _deceleration;
        maxSpeed = _maxSpeed;
        playerCollider = _collider;
    }

    public bool IsSliding { get; set; }
    public bool IsCrouching { get; set; }
    public bool ForcedCrouch { get; set; }

    public Animator Animator {get; set;}
    void Move(Rigidbody2D rb, Vector2 input);

    IEnumerator SlideCor(Rigidbody2D rb, Vector2 input);

    Coroutine slideCoroutine { get; set; }
    void StartSlide(Rigidbody2D rb, Vector2 input);
    void StopSlide(Rigidbody2D rb);

    void Crouch();
    void UnCrouch();
}
