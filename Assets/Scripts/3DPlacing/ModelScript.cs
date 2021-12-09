using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put this in the top empty gameobject
public class ModelScript : MonoBehaviour
{
    public GameObject BottomAnchor;
    public GameObject MidAnchor;
    public Vector2 MidPoint;

    public bool UsingBottomAnchor { get; private set; }
    public Vector3 Size { get; private set; }

    private float _sizeMultiplier = 1;
    public float SizeMultiplier { 
        get => _sizeMultiplier;
        set
        {
            if (_sizeMultiplier == value)
                return;

            _sizeMultiplier = value;
            if (UsingBottomAnchor)
                this.BottomAnchor.transform.localScale = new Vector3(value, value, value);
            else
                this.MidAnchor.transform.localScale = new Vector3(value, value, value);
        }
    }

    private float _rotation = 0;
    public float Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation == value)
                return;

            _rotation = value;
            this.gameObject.transform.eulerAngles = new Vector3(0, value, 0);
        }
    }

    protected virtual void Awake()
    {
        CalculateSize();
        BottomAnchor.transform.parent = gameObject.transform.parent;
        BottomAnchor.transform.localPosition = gameObject.transform.localPosition;
        UsingBottomAnchor = true;
        UseMidAnchor();
    }

    public virtual void UseBottomAchor()
    {
        if (UsingBottomAnchor)
            return;

        BottomAnchor.transform.parent = MidAnchor.transform.parent;
        BottomAnchor.transform.localPosition = MidAnchor.transform.localPosition;
        gameObject.transform.parent = null;

        MidAnchor.transform.parent = gameObject.transform;

        gameObject.transform.parent = BottomAnchor.transform;

        BottomAnchor.transform.localScale = new Vector3(_sizeMultiplier, _sizeMultiplier, 1);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        UsingBottomAnchor = true;
    }

    public virtual void UseMidAnchor()
    {
        if (!UsingBottomAnchor)
            return;

        MidAnchor.transform.parent = BottomAnchor.transform.parent;
        MidAnchor.transform.localPosition = BottomAnchor.transform.localPosition;

        gameObject.transform.parent = null;

        BottomAnchor.transform.parent = gameObject.transform;

        gameObject.transform.parent = MidAnchor.transform;

        MidAnchor.transform.localScale = new Vector3(_sizeMultiplier, _sizeMultiplier, 1);
        gameObject.transform.localPosition = new Vector3(MidPoint.x, -MidPoint.y, 0);
        UsingBottomAnchor = false;
    }

    public void CalculateSize()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        if (meshes.Length == 0)
        {
            Size = new Vector3(0, 0, 0);
            return;
        }

        float minX, minY, minZ = 0;
        float maxX, maxY, maxZ = 0;

        minX = meshes[0].bounds.min.x;
        minY = meshes[0].bounds.min.y;
        minZ = meshes[0].bounds.min.z;

        maxX = meshes[0].bounds.max.x;
        maxY = meshes[0].bounds.max.y;
        maxZ = meshes[0].bounds.max.z;

        for (int i = 1; i < meshes.Length; i++)
        {
            var minVec = meshes[i].bounds.min;
            var maxVec = meshes[i].bounds.max;

            if (minX > minVec.x)
                minX = minVec.x;
            if (minY > minVec.y)
                minY = minVec.y;
            if (minZ > minVec.z)
                minZ = minVec.z;

            if (maxX < maxVec.x)
                maxX = maxVec.x;
            if (maxY < maxVec.y)
                maxY = maxVec.y;
            if (maxZ < maxVec.z)
                maxZ = maxVec.z;
        }

        Size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
    }

    public void MoveTo(Vector3 position)
    {
        if (UsingBottomAnchor)
        {
            BottomAnchor.transform.position = position;
            return;
        }

        MidAnchor.transform.position = position;
    }
}
