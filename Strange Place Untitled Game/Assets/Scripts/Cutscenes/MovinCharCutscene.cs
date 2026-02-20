using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MovinCharCutscene : MonoBehaviour
{
    [SerializeField] bool goingToRight;
    public float moveDistance = 5f; 
    public float jumpHeight = 0.5f; 
    public float jumpsDuration = 0.3f; 
    public int jumps = 10;      
    Vector2 originalScale;

    public bool inAnimation;


    void Awake()
    {
        originalScale=transform.localScale;
    }

    public IEnumerator playAnimation(){
        inAnimation = true;
        if(goingToRight){
            transform.localScale=originalScale;
        }
        else{
            transform.localScale= new Vector2(originalScale.x*-1,originalScale.y);
        }

        yield return StartCoroutine(JumpAnimation(goingToRight));
        inAnimation = false;
    }

    IEnumerator JumpAnimation(bool isRight)
    {
        float direction = isRight ? 1f : -1f;
        float distancePerJump = moveDistance / jumps;
        
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < jumps; i++)
        {
            float targetX = transform.position.x + (distancePerJump * (i + 1) * direction);
            float targetY = transform.position.y + jumpHeight;
            sequence.Append(transform.DOMoveY(targetY, jumpsDuration / 2).SetEase(Ease.OutQuad));
            sequence.Append(transform.DOMoveY(transform.position.y, jumpsDuration / 2).SetEase(Ease.InQuad));
            sequence.Join(transform.DOMoveX(targetX, jumpsDuration).SetEase(Ease.Linear));
        }
        
        sequence.Play();
        yield return sequence.WaitForCompletion(); 
    }
}
