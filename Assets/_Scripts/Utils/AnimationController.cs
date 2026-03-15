using DG.Tweening;
using System;
using System.Collections;
using UnityEngine.UI;

public static class AnimationController
{
    public static Tween Fade<T>(Tween tween, T uiItem, float endValue, float duration) 
        where T : Graphic
    {
        tween?.Kill();

        tween = uiItem.DOFade(endValue, duration);
        return tween;
    }

    public static Tween FadeInAndOut<T>(Tween tween, T uiItem, float duration, float interval)
        where T : Graphic
    {
        tween?.Kill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(uiItem.DOFade(1f, duration));
        sequence.AppendInterval(interval);
        sequence.Append(uiItem.DOFade(0f, duration));
        
        tween = sequence;
        return tween;
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
