using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPanelLookAt : MonoBehaviour
{
    public Transform lookAt;
    public Camera cam;
    private Vector3 offset;
    private ModelScript model;

    void Update()
    {
        model ??= lookAt.GetComponent<ModelScript>();
        offset = (model.UsingBottomAnchor ? model.Size.y : model.Size.y / 2f + model.Size.y / 3f) * Vector3.up;
        var position = cam.WorldToScreenPoint(lookAt.position + offset);
        var rect = GetComponent<RectTransform>();
        
        if (position != rect.anchoredPosition3D)
        {
            Debug.Log($"model: {model.Size.y}");
            rect.anchoredPosition3D = position;
        }
    }
}