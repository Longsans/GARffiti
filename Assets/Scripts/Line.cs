using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Line
{
    private LineRenderer _lineRend;
    private GameObject _gameObject;
    public GameObject GameObject { get => _gameObject; }

    public Transform transform => _lineRend.transform;

    public Line(GameObject linePrefab, Vector3 position, ARPlane planeAttachedTo)
    {
        _gameObject = ARCursor.Instantiate(linePrefab);
        _lineRend = _gameObject.GetComponent<LineRenderer>();
        _lineRend.positionCount = 0;
        AlignToPlane(planeAttachedTo);
        DrawTo(position);
    }

    public void DrawTo(Vector3 position)
    {
        _lineRend.SetPosition(_lineRend.positionCount++, position);
    }

    public void DrawInDirection(Vector3 movement)
    {
        _lineRend.transform.position += movement;
    }

    public void SetColor(Color color)
    {
        _lineRend.sharedMaterial.color = color;
    }

    public void SetWidth(float multiplier)
    {
        _lineRend.widthMultiplier = multiplier;
    }

    private void AlignToPlane(ARPlane plane)
    {
        _lineRend.transform.rotation = Quaternion.LookRotation(-plane.normal, _lineRend.transform.up);
    }
}
