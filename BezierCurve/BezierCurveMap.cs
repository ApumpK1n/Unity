using UnityEngine;
using System.Collections;


public sealed class BezierCurveMap
{
    private readonly float[] _arcLengths;
    private readonly float _ratio;
    public float length { get; private set; }
    public BezierCurve curve { get; private set; }
    public bool isSet { get { return length != float.NaN; } }
    public int resolution { get { return _arcLengths.Length; } }

    public BezierCurveMap(int resolution)
    {
        _arcLengths = new float[resolution];
        _ratio = 1f / resolution;
        length = float.NaN;
    }

    public void Set(BezierCurve c)
    {
        curve = c;
        Vector2 o = c.Sample(0);
        float ox = o.x;
        float oy = o.y;
        float clen = 0;
        int nSamples = _arcLengths.Length;
        for (int i = 0; i < nSamples; i++)
        {
            float t = (i + 1) * _ratio;
            Vector2 p = c.Sample(t);
            float dx = ox - p.x;
            float dy = oy - p.y;
            clen += Mathf.Sqrt(dx * dx + dy * dy);
            _arcLengths[i] = clen;
            ox = p.x;
            oy = p.y;
        }
        length = clen;
    }

    public Vector2 Sample(float u)
    {
        if (u <= 0) return curve.Sample(0);
        if (u >= 1) return curve.Sample(1);

        int index = 0;
        int low = 0;
        int high = resolution - 1;
        float target = u * length;
        float found = float.NaN;

        // 找到距离近似值
        while (low < high)
        {
            index = (low + high) / 2;
            found = _arcLengths[index];
            if (found < target)
                low = index + 1;
            else
                high = index;
        }

        if (found > target)
            index--;

        if (index < 0) return curve.Sample(0);
        if (index >= resolution - 1) return curve.Sample(1);

        // 找出时间近似值并做一个修正
        float min = _arcLengths[index];
        float max = _arcLengths[index + 1];
        float interp = (target - min) / (max - min);
        return curve.Sample((index + interp + 0.5f) * _ratio);
    }
}