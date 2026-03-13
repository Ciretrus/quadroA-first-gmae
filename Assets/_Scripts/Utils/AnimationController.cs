using UnityEngine;
using System.Collections;
using System;

// TODO Remove it completely/rework
public static class AnimationController
{
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

    public static IEnumerator AnimateTransitionWithPause(
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
