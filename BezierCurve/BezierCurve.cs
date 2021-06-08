using UnityEngine;
using System.Collections;


public class BezierCurve
{

    private Vector3 start;
    private Vector3 end;
    private Vector3 center;


    public BezierCurve(Vector3 start, Vector3 center, Vector3 end)
    {
        this.start = start;
        this.center = center;
        this.end = end;
    }


    public Vector3 Sample(float t)
    {
        return Bezier3Point(start, center, end, t);
    }

    public Vector3 Bezier3Point(Vector3 start, Vector3 center, Vector3 end, float t)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }
}