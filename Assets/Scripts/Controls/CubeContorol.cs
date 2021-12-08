
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CubeContorol : UdonSharpBehaviour
{
    [SerializeField, Header("回転スピード")] private float rotateSpeed = 0.0f;

    private void Update()
    {
        // オブジェクトを上軸中心回転を行う
        this.gameObject.transform.Rotate(this.gameObject.transform.up * rotateSpeed * Time.deltaTime);
    }
}
