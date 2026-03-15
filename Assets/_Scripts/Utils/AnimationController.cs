using DG.Tweening;
using System;
using System.Collections;
using UnityEngine.UI;

public static class AnimationController
{
    public static void Fade<T>(Tween tween, T uiItem, float endValue, float duration) 
        where T : Graphic
    {
        tween?.Kill();

        tween = uiItem.DOFade(endValue, duration);
    }

    public static void FadeInAndOut<T>(Tween tween, T uiItem, float duration, float interval)
        where T : Graphic
    {
        tween?.Kill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(uiItem.DOFade(1f, duration));
        sequence.AppendInterval(interval);
        sequence.Append(uiItem.DOFade(0f, duration));
        
        tween = sequence;
    }

    public static IEnumerator FadeInAndOut<T>(
        Action mainAction, 
        Action action, 
        Tween tween, 
        T uiItem, 
        float duration, 
        float interval)
        where T : Graphic
    {
        tween?.Kill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(uiItem.DOFade(1f, duration));
        action();
        yield return sequence.AppendInterval(interval).WaitForCompletion(); 
        mainAction();
        action();
        sequence.Append(uiItem.DOFade(0f, duration));

        tween = sequence;
    }
}
