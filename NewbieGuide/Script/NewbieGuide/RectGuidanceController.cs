using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 矩形引导组件
/// </summary>
public class RectGuidanceController : Guidance
{

    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 当前的偏移值X
    /// </summary>
    private float _currentOffsetX = 0f;

    /// <summary>
    /// 当前的偏移值Y
    /// </summary>
    private float _currentOffsetY = 0f;

    public override void RefreshMask()
    {
        target.GetWorldCorners(_corners);

        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        float z = (_corners[0].z + _corners[1].z + _corners[2].z + _corners[3].z) / 4;
        Vector3 centerWorld = new Vector3(x, y, z);
        _center = targetCamera.WorldToScreenPoint(centerWorld);
        _material.SetVector("_Center", _center);
        _currentOffsetX = 0;
        _currentOffsetY = 0;
        for (int i = 0; i < _corners.Length; i++)
        {
            if (i % 2 == 0)
                _currentOffsetX = Mathf.Max(Mathf.Abs(targetCamera.WorldToScreenPoint(_corners[i]).x - targetCamera.WorldToScreenPoint(centerWorld).x), _currentOffsetX);
            else
                _currentOffsetY = Mathf.Max(Mathf.Abs(targetCamera.WorldToScreenPoint(_corners[i]).y - targetCamera.WorldToScreenPoint(centerWorld).y), _currentOffsetY);
        }
        //设置遮罩材质中当前偏移的变量
        _material.SetFloat("_SliderX", _currentOffsetX);
        _material.SetFloat("_SliderY", _currentOffsetY);
    }

    public override bool IsInActiveArea(Vector3 position)
    {
        Vector3 dis = position - _center;
        return !(Mathf.Abs(dis.x) > _currentOffsetX || Mathf.Abs(dis.y) > _currentOffsetY);
    }
}