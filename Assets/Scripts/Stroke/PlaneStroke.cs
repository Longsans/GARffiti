using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneStroke : Stroke
{
    private LineRenderer _lineRend;

    private ARPlane _plane;
    public ARPlane Plane 
    {
        get => _plane;
        set
        {
            if (_plane == value)
                return;

            _plane = value;
            if (_plane != null)
                _lineRend.transform.rotation = Quaternion.LookRotation(-_plane.normal, _lineRend.transform.up);
        }
    }

    public PlaneStroke(GameObject brushObject, ARPlane plane) : base(brushObject)
    {
        _lineRend = GameObject.GetComponent<LineRenderer>();
        _lineRend.positionCount = 0;
        _lineRend.sharedMaterial = Material;
        Plane = plane;
    }

    public override void DrawTo(Vector3 position)
    {
        _lineRend.SetPosition(_lineRend.positionCount++, position);
    }

    public override void DrawInDirection(Vector3 movement)
    {
        _lineRend.transform.position += movement;
    }

    public override void SetColor(Color color)
    {
        _lineRend.sharedMaterial.color = color;
    }

    public override void SetWidth(float multiplier)
    {
        _lineRend.widthMultiplier = multiplier;
    }

    public override void SetMaterial(Material material)
    {
        base.SetMaterial(material);
        _lineRend.sharedMaterial = material;
    }

    public override void Finished()
    {
        Vector3[] vectors = new Vector3[_lineRend.positionCount];
        _lineRend.GetPositions(vectors);
        _lineRend.positionCount = 0;

        List<Vector3> newPoints = CurveMath.GenerateBezierCurve(vectors, 0.01f);
        for (int i = 0; i < newPoints.Count; i++)
        {
            _lineRend.positionCount++;
            _lineRend.SetPosition(i, newPoints[i]);
        }

        base.Finished();
    }
}
