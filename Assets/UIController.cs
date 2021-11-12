using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image DrawModeImage;

    private ARCursor.__DrawMode _drawMode;

    private void Start()
    {
        _drawMode = FindObjectOfType<ARCursor>().DrawMode;
    }

    public void ChangeDrawModeIcon()
    {
        if (_drawMode == ARCursor.__DrawMode.SpaceOnly)
        {
            _drawMode = ARCursor.__DrawMode.PlanesOnly;
            DrawModeImage.sprite = Resources.Load<Sprite>("Icons/black-trap");
        }
        else
        {
            _drawMode = ARCursor.__DrawMode.SpaceOnly;
            DrawModeImage.sprite = Resources.Load<Sprite>("Icons/3d-cube");
        }
        FindObjectOfType<ARCursor>().DrawMode = _drawMode;
    }
}
