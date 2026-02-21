using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject jumpPrefab;
    [SerializeField] GameObject runPrefab;

    [SerializeField] float suctionForce;
    [SerializeField] float suctionSpeed;
       
    [SerializeField] string nextSceneName;
    [SerializeField] string phaseName;

     void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!other.CompareTag("Player"))
        return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
            pm.canMove = false;

        GameRunTimer.Instance.CompletePhase(phaseName);

        if (string.IsNullOrEmpty(nextSceneName))
        {
            GameRunTimer.Instance.StopRun();
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
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
