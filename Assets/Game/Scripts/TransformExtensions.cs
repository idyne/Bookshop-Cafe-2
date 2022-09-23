using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class TransformExtensions
{
    private static AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.5f), new Keyframe(1, 0));

    public static void ParabolicMove(this Transform transform, Vector3 to, float time)
    {
        ParabolicMove(transform, to, time, () => { });
    }
    public static void ParabolicMove(this Transform transform, Vector3 to, float time, TweenCallback onComplete)
    {
        Vector3 start = transform.position;
        DOTween.To((val) =>
        {
            Vector3 end = to;
            Vector3 pos = Vector3.Lerp(start, end, val);
            pos.y += curve.Evaluate(val);
            transform.position = pos;
        }, 0, 1, time).OnComplete(onComplete);
    }
}
