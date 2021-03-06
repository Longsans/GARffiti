using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put this in the top empty gameobject
public class ModelScript : MonoBehaviour
{
    public GameObject BottomAnchor;
    public GameObject MidAnchor;
    public Vector2 MidPoint;
    public Vector2 BottomPoint;

    public GameObject EditPanel;
    public bool AutoCalculateSize = false;

    public bool UsingBottomAnchor { get; private set; }
    public Vector3 Size;

    public Vector3 InstanceRotation => instanceRot;
    private Vector3 instanceRot;

    public Vector3 LastInstRotation { get; private set; }

    // Will be record when finish
    private Vector3 _lastPosition;
    private bool _finished = false;

    private float _sizeMultiplier = 1;
    public float SizeMultiplier { 
        get => _sizeMultiplier;
        set
        {
            if (_sizeMultiplier == value)
                return;

            _sizeMultiplier = value;
            if (UsingBottomAnchor)
                this.BottomAnchor.transform.localScale = value * Vector3.one;
            else
                this.MidAnchor.transform.localScale = value * Vector3.one;
        }
    }
    public float LastSizeMultiplier { get; private set; }

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

    public Vector3 Location
    { 
        get
        {
            if (UsingBottomAnchor)
                return BottomAnchor.transform.position;
            else
                return MidAnchor.transform.position;
        }
    }

    public void Show()
    {
        if (UsingBottomAnchor)
        {
            BottomAnchor.SetActive(true);
        }
        else
        {
            MidAnchor.SetActive(true);
        }
    }

    public void Hide()
    {
        if (UsingBottomAnchor)
        {
            BottomAnchor.SetActive(false);
        }
        else
        {
            MidAnchor.SetActive(false);
        }
    }

    protected virtual void Awake()
    {
        if (AutoCalculateSize)
            CalculateSize();
        BottomAnchor.transform.parent = gameObject.transform.parent;
        BottomAnchor.transform.localPosition = gameObject.transform.localPosition;
        UsingBottomAnchor = true;
        UseMidAnchor();
        instanceRot = new Vector3(0, 0, 0);
    }

    public virtual void UseBottomAchor()
    {
        if (UsingBottomAnchor)
            return;

        BottomAnchor.transform.parent = MidAnchor.transform.parent;
        BottomAnchor.transform.localPosition = MidAnchor.transform.localPosition;
        BottomAnchor.SetActive(MidAnchor.activeSelf);

        gameObject.transform.parent = null;

        MidAnchor.transform.parent = gameObject.transform;
        MidAnchor.SetActive(true);

        gameObject.transform.parent = BottomAnchor.transform;

        BottomAnchor.transform.localScale = new Vector3(_sizeMultiplier, _sizeMultiplier, _sizeMultiplier);
        gameObject.transform.localPosition = new Vector3(BottomPoint.x, -BottomPoint.y, 0);
        UsingBottomAnchor = true;
    }

    public virtual void UseMidAnchor()
    {
        if (!UsingBottomAnchor)
            return;

        MidAnchor.transform.parent = BottomAnchor.transform.parent;
        MidAnchor.transform.localPosition = BottomAnchor.transform.localPosition;
        MidAnchor.SetActive(BottomAnchor.activeSelf);

        gameObject.transform.parent = null;

        BottomAnchor.transform.parent = gameObject.transform;
        BottomAnchor.SetActive(true);

        gameObject.transform.parent = MidAnchor.transform;

        MidAnchor.transform.localScale = new Vector3(_sizeMultiplier, _sizeMultiplier, _sizeMultiplier);
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

    public void MoveToLocal(Vector3 position)
    {
        if (UsingBottomAnchor)
        {
            BottomAnchor.transform.localPosition = position;
            return;
        }

        MidAnchor.transform.localPosition = position;
    }

    public void OnSizeChanged(float scaleFactor)
    {
        SizeMultiplier = scaleFactor;
        CalculateSize();
    }

    public void OnRotationXChanged(float x)
    {
        transform.eulerAngles = new Vector3(x, instanceRot.y, instanceRot.z);
        instanceRot.x = x;
        CalculateSize();
    }

    public void OnRotationYChanged(float y)
    {
        transform.eulerAngles = new Vector3(instanceRot.x, y, instanceRot.z);
        instanceRot.y = y;
        CalculateSize();
    }

    public void OnRotationZChanged(float z)
    {
        transform.eulerAngles = new Vector3(instanceRot.x, instanceRot.y, z);
        instanceRot.z = z;
        CalculateSize();
    }

    public void MarkLastSizeMultiplier()
    {
        LastSizeMultiplier = SizeMultiplier;
    }

    public void MarkLastRotation()
    {
        LastInstRotation = InstanceRotation;
    }

    public void Finish()
    {
        if (_finished)
            History.AddAction(new RelocateAction(this, _lastPosition, Location));
        else
        {
            History.AddAction(new PlacingAction(this));
            _finished = true;
        }

        _lastPosition = Location;
    }

    public void Destroy()
    {
        Destroy(EditPanel);
        if (UsingBottomAnchor)
            GameObject.Destroy(BottomAnchor);
        else
            GameObject.Destroy(MidAnchor);
    }
}
