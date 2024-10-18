using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Camera mainCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void ShakeCamera(float shakePower, float shakeDuration)
    {
        mainCamera.transform.DOShakeRotation(shakeDuration, shakePower);
    }
}
