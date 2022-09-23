using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UITutorial : MonoBehaviour
{
    private void Start()
    {
        Vector3 pos = transform.localPosition;
        pos.x = -130;
        transform.localPosition = pos;
        transform.DOLocalMoveX(130, 1.2f).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo);
    }
}
