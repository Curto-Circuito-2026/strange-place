using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject jumpPrefab;
    [SerializeField] GameObject runPrefab;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("TROCANDO");
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            pm.SetJump(jumpPrefab);
            pm.SetRun(runPrefab);
        }
    }
}
