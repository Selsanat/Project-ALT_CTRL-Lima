using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using DG.Tweening;

public class ArrowJump : MonoBehaviour
{
    void Start()
    {
        transform.DOJump(transform.position, 2, 1, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuint);
        transform.DOShakeScale(0.5f, 0.05f, 1, 0).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuint);
    }
}
