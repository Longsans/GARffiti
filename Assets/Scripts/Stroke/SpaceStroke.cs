using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStroke : Stroke
{
    private TrailRenderer _trailRend;

    public SpaceStroke(GameObject brushObject) : base(brushObject)
    {
        _trailRend = GameObject.GetComponent<TrailRenderer>();
        _trailRend.Clear();
        _trailRend.sharedMaterial = Material;
    }

    public override void DrawTo(Vector3 position)
    {
        _trailRend.gameObject.transform.position = position;
    }

    public override void DrawInDirection(Vector3 movement)
    {
        _trailRend.transform.position += movement;
    }

    public override void SetColor(Color color)
    {
        _trailRend.sharedMaterial.color = color;
    }

    public override void SetWidth(float multiplier)
    {
        _trailRend.widthMultiplier = multiplier;
    }

    public override void SetMaterial(Material material)
    {
        base.SetMaterial(material);
        _trailRend.sharedMaterial = material;
    }

    public override void Finished()
    {
        Vector3[] vectors = new Vector3[_trailRend.positionCount];
        _trailRend.GetPositions(vectors);
        List<Vector3> newPoints = CurveMath.GenerateBezierCurve(vectors, 0.1f);
        _trailRend.AddPositions(newPoints.ToArray());

        base.Finished();
    }
}
