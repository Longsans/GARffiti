using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stroke : IDisposable
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

    private Vector3[] _segment = new Vector3[3];
    private int _currentSegmentIndex = 0;

    public Stroke(GameObject brushObject)
    {
        _gameObject = brushObject;
        _lineRend = GameObject.GetComponent<LineRenderer>();
        _lineRend.positionCount = 0;
        _lineRend.sharedMaterial = Material;

        MinDistance = 0.05f;
    }

    public List<Vector3> GenerateBezierCurve(IEnumerable<Vector3> vector3s)
    {
        return null;
    }

    public virtual void StartDraw(Vector3 position)
    {
        DrawTo(position);
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

            if (_currentSegmentIndex == _segment.Length)
            {
                _segment[_segment.Length - 1] = GeneralMath.GetMidPoint(_segment[_segment.Length - 1], position);
                List<Vector3> newPoints = CurveMath.GenerateBezierCurveCasteljau(_segment, 0.1f);

                _lineRend.positionCount -= _segment.Length;
                for (int i = 0; i < newPoints.Count; i++)
                {
                    _lineRend.SetPosition(_lineRend.positionCount++, newPoints[i]);
                }

                _currentSegmentIndex = 0;
            }

            _segment[_currentSegmentIndex] = position;
            _currentSegmentIndex++;
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
        if (_currentSegmentIndex > 2)
        {
            List<Vector3> newPoints = CurveMath.GenerateBezierCurveCasteljau(new List<Vector3>(_segment).GetRange(0, _currentSegmentIndex), 0.1f);
            _lineRend.positionCount -= _currentSegmentIndex;
            for (int i = 0; i < newPoints.Count; i++)
            {
                _lineRend.SetPosition(_lineRend.positionCount++, newPoints[i]);
            }
        }

        // Have to call before store
        ARCursor.Instance.CurrentStroke = this;

        // Save to history put here
        DrawAction drawAction = new DrawAction(this, ARCursor.Instance.CurrentStroke);
        History.AddAction(drawAction);
    }

    public void Hide()
    {
        _gameObject.SetActive(false);
    }

    public void Show()
    {
        _gameObject.SetActive(true);
    }

    public void Dispose()
    {
        GameObject.Destroy(_gameObject);
    }
}
