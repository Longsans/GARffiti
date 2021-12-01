using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stroke
{
    private GameObject _gameObject;
    public GameObject GameObject { get => _gameObject; }
    public Transform transform => _gameObject.transform;

    private Material _material;
    public Material Material { get => _material; }

    public float BezierStep { get; set; }

    public Stroke(GameObject brushObject)
    {
        _gameObject = brushObject;
    }

    public abstract void DrawTo(Vector3 position);
    public abstract void DrawInDirection(Vector3 movement);
    public abstract void SetColor(Color color);
    public abstract void SetWidth(float multiplier);
    public virtual void SetMaterial(Material material)
    {
        _material = material;
    }
    public virtual void Finished()
    {
        // Add action to history here

    }

    public List<Vector3> GenerateBezierCurve(IEnumerable<Vector3> vector3s)
    {
        return null;
    }
}
