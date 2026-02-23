using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalanceSystem : MonoBehaviour
{
    [Header("Equilibrio")]
    [Range(-1.0f, 1.0f)] public float balanceValue = 0; 

    [SerializeField] float maxBalanceValue = 1;
    [SerializeField] float gravityForce = 0.8f;      
    [SerializeField] float recoveryForce = 2.5f;  
    [SerializeField] float runInstability = 0.15f;    
    [SerializeField] float jumpInstability = 0.4f;    
    [SerializeField] float slideInstability = 0.6f; 

    [Header("Visual")]
    [SerializeField] Transform container; 
    [SerializeField] float maxVisualAngle = 30;


    LifeSystem playerLife;

    private PlayerMovement movement;
    private Rigidbody2D rb;
    private float playerInput;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerLife = GetComponent<LifeSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        movement.OnPlayerJump += ApplyJumpImpact;
        movement.OnPlayerSlide += ApplySlideImpact;
        movement.OnPlayerRun += ApplyRunImpact;
    }
    public void OnBalance(InputValue value)
    {
        playerInput = value.Get<float>();
    }

    void Update()
    {
        if (!movement.canMove) return;

        if (playerInput != 0)
        {
            balanceValue += playerInput * recoveryForce * Time.deltaTime;
        }
        
        ApplyNaturalPhysics();
        UpdateContainer();
        CheckFallCondition();
        GameRunTimer.Instance.balance.UpdateBalance(balanceValue);
    }


    private void ApplyRunImpact()
    {
        float horizontalVel = rb.linearVelocity.x;
        balanceValue -= horizontalVel * runInstability * Time.fixedDeltaTime;
    }

    private void ApplyJumpImpact()
    {
        float moveDir = rb.linearVelocity.x;
        float force;

        if (Mathf.Abs(moveDir) > 0.1f)
        {
            force = (moveDir > 0 ? -1 : 1) * jumpInstability;
        }
        else
        {
            force = Random.Range(-1f, 1f) * jumpInstability;
        }

        balanceValue += force;
    }

    private void ApplySlideImpact()
    {
        float moveDir = rb.linearVelocity.x;
        
        if (Mathf.Abs(moveDir) > 0.1f)
        {
            float slideForce = (moveDir > 0 ? -1 : 1) * slideInstability;
            balanceValue += slideForce;
        }
    }

    private void ApplyNaturalPhysics()
    {
        if (balanceValue != 0)
        {
            float gravityEffect = -(balanceValue > 0 ? gravityForce : -gravityForce) * Time.deltaTime;
            balanceValue += gravityEffect;
        }

        balanceValue = Mathf.Clamp(balanceValue, -1.2f, 1.2f);
    }

    private void UpdateContainer()
    {
        if (container != null)
        {
            float zRotation = -balanceValue * maxVisualAngle;
            container.localRotation = Quaternion.Euler(0, 0, zRotation);
        }
    }

    private void CheckFallCondition()
    {
        if (Mathf.Abs(balanceValue) > maxBalanceValue)
        {
            playerLife.OnRevive -= RestartBalance; 
            playerLife.OnRevive+=RestartBalance;
            playerLife.GetDamage(1000);
        }
    }

    void RestartBalance()
    {
        balanceValue = 0;
    }
}