using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurveMath
{
    public static List<Vector3> GenerateBezierCurve(IEnumerable<Vector3> pointsCollection, float step)
    {
        List<Vector3> points = new List<Vector3>(pointsCollection);
        List<Vector3> controlPoints = new List<Vector3>();
        List<Vector3> result = new List<Vector3>();

        for (int i = 4; i < points.Count - 1; i += 4)
        {
            points[i] = GeneralMath.GetMidPoint(points[i - 1], points[i + 1]);
        }

        controlPoints.Add(points[0]);
        for (int i = 0; i < points.Count - 1; i++)
        {
            controlPoints.Add(GeneralMath.GetMidPoint(points[i], points[i + 1]));
        }
        controlPoints.Add(points[points.Count - 1]);

        for (int i = 0; i < controlPoints.Count - 3; i += 3)
        {
            for (float t = 0; t < 1; t += step)
            {
                result.Add(CalculateCubicBezierPoint(t, controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3]));
            }
        }
        return result;
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
