using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneStroke : Stroke
{
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
                LineRenderer.transform.rotation = Quaternion.LookRotation(-_plane.normal, LineRenderer.transform.up);
        }
    }

    public PlaneStroke(GameObject brushObject, ARPlane plane) : base(brushObject)
    {
        Plane = plane;
        LineRenderer.alignment = LineAlignment.TransformZ;
    }
}
