using UnityEngine;

public interface IJump
{
    public bool CanJump { get; }
    void Jump(Rigidbody2D rb);
}