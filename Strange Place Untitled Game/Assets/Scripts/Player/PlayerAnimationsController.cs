using UnityEngine;

public class NervousSystem : MonoBehaviour
{

    [SerializeField] AnimatorOverrideController normalAnimator;
    [SerializeField] AnimatorOverrideController nervousAnimator;

    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void SetAnimator(bool nervous)
    {
        if(nervous) animator.runtimeAnimatorController = nervousAnimator;
        else animator.runtimeAnimatorController = normalAnimator;
    }
}
