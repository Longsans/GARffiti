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
        UpdateLookAt();
    }

    public void UpdateLookAt()
    {
        model ??= lookAt.GetComponent<ModelScript>();
        offset = model.Size.y * Vector3.up;
        var position = cam.WorldToScreenPoint(lookAt.position + offset);
        var rect = GetComponent<RectTransform>();

        if (position != rect.anchoredPosition3D)
        {
            rect.anchoredPosition3D = position;
        }
    }
}
