using UnityEngine;

public interface IJump
{
    public bool CanJump { get; }
    public Animator Animator {set;}
    void Jump(Rigidbody2D rb);

    public float jumpForce { get; set; }
    public float wallReflectForce { get; set; }

    public LayerMask groundLayer { get; set; }

    public LayerMask wallLayer { get; set; }


    public void SetValues(float _jumpForce, float _wallReflectForce, LayerMask _groundLayer, LayerMask _wallLayer)
    {
        jumpForce = _jumpForce;
        wallReflectForce = _wallReflectForce;
        groundLayer = _groundLayer;
        wallLayer = _wallLayer;
    }
}