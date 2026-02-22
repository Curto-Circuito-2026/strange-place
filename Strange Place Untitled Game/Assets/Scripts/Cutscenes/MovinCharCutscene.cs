using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MovinCharCutscene : MonoBehaviour
{
    [SerializeField] bool goingToRight;
    [SerializeField] Transform targetDestination; 
    [SerializeField] Sprite afterSpriteDialog;
    
    public float jumpHeight = 50f; 
    public float totalDuration = 2f; 
    public int numJumps = 5;        
    
    Vector2 originalScale;
    public bool inAnimation;
    Sprite original;
    Image sr;

    void Awake()
    {
        sr = GetComponent<Image>();
        if(afterSpriteDialog == null) afterSpriteDialog = sr.sprite;
        original = sr.sprite;
        originalScale = transform.localScale;
    }

    public IEnumerator playAnimation()
    {
        if (targetDestination == null) yield break;

        inAnimation = true;

        float distance = Mathf.Abs(targetDestination.position.x - transform.position.x);
        float direction = goingToRight ? 1f : -1f;
        float targetX = transform.position.x + (distance * direction);

        transform.localScale = new Vector2(originalScale.x * direction, originalScale.y);

        yield return StartCoroutine(JumpToTargetAnimation(targetX));
        
        inAnimation = false;
    }

    IEnumerator JumpToTargetAnimation(float endX)
    {
        Sequence sequence = DOTween.Sequence();
        float jumpDuration = totalDuration / numJumps;
        float startY = transform.position.y;

        sequence.Append(transform.DOMoveX(endX, totalDuration).SetEase(Ease.Linear));

        for (int i = 0; i < numJumps; i++)
        {
            float jumpStartTime = i * jumpDuration;

            sequence.Insert(jumpStartTime, 
                transform.DOMoveY(startY + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad));
            
            sequence.Insert(jumpStartTime + (jumpDuration / 2), 
                transform.DOMoveY(startY, jumpDuration / 2).SetEase(Ease.InQuad));
        }

        yield return sequence.Play().WaitForCompletion();
    }

    public void ChangeSprite(bool useOriginal)
    {
        sr.sprite = useOriginal ? original : afterSpriteDialog;
    }

    public void InvertDirection()
    {
        goingToRight = !goingToRight;
    }
}