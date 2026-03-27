using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour
{
    public GameObject MainCamera;
    private Camera _camera;

    void Start()
    {
        _camera = MainCamera.GetComponent<Camera>();
    }

    // 摄像机大小先变成5，再移动到0,0,-10
    public void FirstAnimation()
    {
        _camera.DOOrthoSize(5f, 0.5f).OnComplete(() =>
            MainCamera.transform.DOMove(new Vector3(0, 0, -10), 0.5f));
    }

    // 摄像机先移动到0,-13.94,-10，再大小变成8.8
    public void SecondAnimation()
    {
        MainCamera.transform.DOMove(new Vector3(0, -13.94f, -10), 0.5f).OnComplete(() =>
            _camera.DOOrthoSize(8.8f, 0.5f));
    }
}
