using UnityEngine;

public interface IJump
{
    public bool CanJump { get; }
    public Animator Animator {set;}
    void Jump(Rigidbody2D rb);
}