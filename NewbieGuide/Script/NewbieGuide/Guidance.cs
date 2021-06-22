using UnityEngine;
using System.Collections;


public class Guidance
{

    /// <summary>
    /// 遮罩材质
    /// </summary>
    protected Material _material;

    /// <summary>
    /// 要高亮显示的目标
    /// </summary>
    protected RectTransform target;

    /// <summary>
    /// 镂空区域中心
    /// </summary>
    protected Vector3 _center;

    protected Canvas canvas;
    protected Camera targetCamera;

    public virtual void RefreshMask()
    {

    }

    public static Vector2 ScreenPosToCanvasPos(RectTransform canvas, Vector3 screenPos)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos,
            canvas.GetComponentInParent<Camera>(), out position);
        return position;
    }

    public void SetMaterial(Material material)
    {
        _material = material;
    }

    public void SetCanvas(Canvas canvas)
    {
        this.canvas = canvas;
    }

    public void SetTarget(RectTransform target)
    {
        this.target = target;
    }
    public void SetCamera(Camera camera)
    {
        targetCamera = camera;
    }

    public virtual bool IsInActiveArea(Vector3 position)
    {
        return false;
    }
}