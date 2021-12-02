using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stroke
{
    private LineRenderer _lineRend;
    public LineRenderer LineRenderer { get => _lineRend; }

    private GameObject _gameObject;
    public GameObject GameObject { get => _gameObject; }
    public Transform transform => _gameObject.transform;

    private Material _material;
    public Material Material { get => _material; }

    public float MinDistance { get => Mathf.Sqrt(_minSqrDistance); set => _minSqrDistance = value * value; }
    private Vector3 _lastPosition;
    // need to store in squared form to reduce the time needed to calculate sqrt
    private float _minSqrDistance;
    // temp is the point that added just to show the user where the line is going but will be removed next draw to
    private bool _tempPos = false;

    public float BezierStep { get; set; }

    public Stroke(GameObject brushObject)
    {
        _gameObject = brushObject;
        _lineRend = GameObject.GetComponent<LineRenderer>();
        _lineRend.positionCount = 0;
        _lineRend.sharedMaterial = Material;

        MinDistance = 0.3f;
    }

    public List<Vector3> GenerateBezierCurve(IEnumerable<Vector3> vector3s)
    {
        return null;
    }

    public virtual void DrawTo(Vector3 position)
    {
        if (_tempPos)
        {
            _lineRend.positionCount--;
            _tempPos = false;
        }

        if (_lastPosition != null && (position - _lastPosition).sqrMagnitude < _minSqrDistance)
        {
            _tempPos = true;
        }
        else
        {
            _lastPosition = position;
        }
        _lineRend.SetPosition(_lineRend.positionCount++, position);
    }

    public virtual void DrawInDirection(Vector3 movement)
    {
        _lineRend.transform.position += movement;
    }

    public virtual void SetColor(Color color)
    {
        _lineRend.sharedMaterial.color = color;
    }

    public virtual void SetWidth(float multiplier)
    {
        _lineRend.widthMultiplier = multiplier;
    }

    public virtual void SetMaterial(Material material)
    {
        _material = material;
        _lineRend.sharedMaterial = material;
    }

    public virtual void Finished()
    {
        // Replace connect point with mid point
        Vector3[] vectors = new Vector3[_lineRend.positionCount];
        _lineRend.GetPositions(vectors);
        _lineRend.positionCount = 0;

        List<Vector3> newPoints = CurveMath.GenerateBezierCurve(vectors, 0.01f);

        for (int i = 0; i < newPoints.Count; i++)
        {
            _lineRend.positionCount++;
            _lineRend.SetPosition(i, newPoints[i]);
        }

        // Save to history put here
    }
}
