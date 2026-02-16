using UnityEngine;

public interface IRun
{
    public Animator Animator {get; set;}
    void Move(Rigidbody2D rb, Vector2 input);
}
