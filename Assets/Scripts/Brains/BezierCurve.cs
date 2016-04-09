using UnityEngine;
using System.Collections;

public class BezierCurve {

    public Vector3[] points;

    public BezierCurve() {
        points = new Vector3[4];
    }

    public void Reset() {
        points = new Vector3[] {
            new Vector3 (1f, 0f, 0f),
            new Vector3 (2f, 0f, 0f),
            new Vector3 (3f, 0f, 0f),
            new Vector3 (4f, 0f, 0f)
        };
    }

    public int ControlPointCount {
        get {
            return points.Length;
        }
    }

    public Vector3 GetPoint(float t) {
        return Bezier.GetPoint(points[0], points[1], points[2], points[3], t);
    }

    public Vector3 GetVelocity(float t) {
        //return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
        return Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t) - new Vector3(0f, 0f, 0f);
    }

    public Vector3 GetDirection(float t) {
        return GetVelocity(t).normalized;
    }
}
