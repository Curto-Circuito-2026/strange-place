using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject jumpPrefab;
    [SerializeField] GameObject runPrefab;

    [SerializeField] float suctionForce;
    [SerializeField] float suctionSpeed;
    [SerializeField] string newSceneName;
 

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.canMove = false;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.SetJump(jumpPrefab);
            pm.SetRun(runPrefab);
            pm.canMove = true;
            //todo
            //remover comentario
            //SceneManager.LoadScene(newSceneName);
            Debug.Log("mandar pra outra scene");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 directionToPortal = (transform.position - other.transform.position).normalized;
                playerRb.AddForce(directionToPortal * suctionForce, ForceMode2D.Force);
                if (playerRb.linearVelocity.magnitude > suctionSpeed)
                {
                    playerRb.linearVelocity = playerRb.linearVelocity.normalized * suctionSpeed;
                }
            }

        }
    }
        
}
