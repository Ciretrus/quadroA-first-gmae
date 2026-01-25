using UnityEngine;
using System.Collections;
using System;

public static class AnimationController
{
    public static void Animate(Animator animator, string animationName)
    {
        animator.Play(animationName);
    }

    public static IEnumerator AnimateWithPause(
        Animator animator, 
        string firstAnimationName, 
        string secondAnimationName,
        float pauseTimer)
    {
        animator.Play(firstAnimationName);
        yield return new WaitForSeconds(pauseTimer);
        animator.Play(secondAnimationName);
    }

    public static IEnumerator AnimateWithPause(
        Animator animator,
        Action mainAction,
        Action action,
        string firstAnimationName,
        string secondAnimationName,
        float pauseTimer)
    {
        animator.Play(firstAnimationName);
        action();
        yield return new WaitForSeconds(pauseTimer);
        mainAction(); 
        action();
        animator.Play(secondAnimationName);
    }
}
