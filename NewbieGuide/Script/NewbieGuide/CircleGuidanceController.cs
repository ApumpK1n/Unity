using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 圆形遮罩镂空引导
/// </summary>
public class CircleGuidanceController : Guidance
{

    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 当前高亮区域的半径
    /// </summary>
    private float _currentRadius;

    /// <summary>
    /// 镂空区域半径
    /// </summary>
    private float _radius;

    public override void RefreshMask()
    {
        _currentRadius = 0;
        target.GetWorldCorners(_corners);

        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        float z = (_corners[0].z + _corners[1].z + _corners[2].z + _corners[3].z) / 4;
        Vector3 centerWorld = new Vector3(x, y, z);
        _center = targetCamera.WorldToScreenPoint(centerWorld);

        Vector4 centerMat = new Vector4(_center.x, _center.y, 0, 0);
        _material.SetVector("_Center", centerMat);
        //计算当前高亮显示区域的半径
        _currentRadius = Mathf.Max(Mathf.Abs(targetCamera.WorldToScreenPoint(_corners[0]).x - _center.x), _currentRadius);
        _currentRadius = Mathf.Max(Mathf.Abs(targetCamera.WorldToScreenPoint(_corners[0]).y - _center.y), _currentRadius);

        _material.SetFloat("_Slider", _currentRadius);
    }

    public override bool IsInActiveArea(Vector3 position)
    {
        float distance = (position - _center).magnitude;
        return distance <= _currentRadius;
    }
}