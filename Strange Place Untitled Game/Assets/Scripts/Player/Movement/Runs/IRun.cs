using UnityEngine;
using System.Collections;

public interface IRun
{
    float maxSpeed { get; set; }
    float acceleration { get; set; }
    float deceleration { get; set; }

    public void SetValues(float _maxSpeed, float _acceleration, float _deceleration)
    {
        acceleration = _acceleration;
        deceleration = _deceleration;
        maxSpeed = _maxSpeed;
    }

    public bool IsSliding { get; set; }

    public Animator Animator {get; set;}
    void Move(Rigidbody2D rb, Vector2 input);

    IEnumerator SlideCor(Rigidbody2D rb, Vector2 input);
}
