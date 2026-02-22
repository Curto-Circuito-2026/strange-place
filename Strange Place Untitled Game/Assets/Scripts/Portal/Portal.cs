using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] float suctionForce;
    [SerializeField] float suctionSpeed;
       
    [SerializeField] string nextSceneName;
    [SerializeField] string phaseName;
    
    [SerializeField] string[] dialogueLines;

    [SerializeField] Sprite dialogSprite;
    bool finishedDialog = false;


    Coroutine coroutine;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") && !finishedDialog)
        return;

        GameRunTimer.Instance.CompletePhase(phaseName);

        if (nextSceneName.ToUpper().Contains("FINAL"))
        {
            GameRunTimer.Instance.StopRun();
        }
        SceneManager.LoadScene(nextSceneName);

    }
    void OnTriggerStay2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null){
                pm.CantWalk();
                }
            if(coroutine==null){
                coroutine = StartCoroutine(DialogCoroutine());
            }

            if(finishedDialog)
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

    IEnumerator DialogCoroutine()
    {
        DialogSystem.Instance.SetActive(true);
        foreach (string rawLine in dialogueLines)
        {
            yield return StartCoroutine(DialogSystem.Instance.TypeDialog(rawLine,dialogSprite));
            yield return new WaitUntil(() => 
                (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) || 
                (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            );

            
        }
        DialogSystem.Instance.SetActive(false);
        yield return new WaitForSeconds(1);
        finishedDialog = true;
    }        
}
