using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    public float dampValue = 0.1f;

    int horizontal = 0;
    int vertical = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("xVelocity");
        vertical = Animator.StringToHash("yVelocity");
    }

    //Play any animation that you want to when called. If isInteracting is true, then you'll be locked in the animation played in the function below
    public void PlayDesiredAnimation(string desiredAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting",isInteracting);
        animator.CrossFade(desiredAnimation, 0.2f);
    }

    public void UpdateAnimationValues(float xVelocity,float yVelocity)
    {
        animator.SetFloat(horizontal, xVelocity, dampValue, Time.deltaTime);
        animator.SetFloat(vertical,yVelocity, dampValue, Time.deltaTime);
    }
}
