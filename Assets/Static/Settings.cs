using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Settings
{
    private static Color _brushColor = new Color(0.2f, 0.7f, 1);
    public static Color BrushColor 
    {
        get => _brushColor;
        set
        {
            if (_brushColor == value)
                return;

            _brushColor = value;
            onBrushColorChanged.Invoke(value);
        }
    }

    private static float _brushWidth = 0.3f;
    public static float BrushWidth 
    {
        get => _brushWidth;
        set
        {
            if (_brushWidth == value)
                return;

            _brushWidth = value;
            onBrushWidthChanged.Invoke(value);
        }
    }

    private static ARCursor.DrawModeType _drawMode;
    public static ARCursor.DrawModeType DrawMode 
    {
        get => _drawMode;
        set
        {
            if (_drawMode == value)
                return;

            _drawMode = value;
            onDrawModeChanged.Invoke(value);
        }
    }

    private static Texture2D _texture;
    public static Texture2D Texture
    {
        get => _texture;
        set
        {
            if (value == _texture)
                return;

            _texture = value;
            onTextureChanged.Invoke(value);
        }
    }

    private static GameObject _selected3DModel;
    public static GameObject Selected3DModel
    {
        get => _selected3DModel;
        set
        {
            if (value == _selected3DModel)
                return;

            _selected3DModel = value;
            onSelected3DModelChanged.Invoke(value);
        }
    }

    public static Material PlaneMaterial { get; set; }
    public static Material PlaneFocusedMaterial { get; set; }

    public static UnityEvent<Texture2D> onTextureChanged = new UnityEvent<Texture2D>();
    public static UnityEvent<float> onBrushWidthChanged = new UnityEvent<float>();
    public static UnityEvent<Color> onBrushColorChanged = new UnityEvent<Color>();
    public static UnityEvent<ARCursor.DrawModeType> onDrawModeChanged = new UnityEvent<ARCursor.DrawModeType>();

    public static UnityEvent<GameObject> onSelected3DModelChanged = new UnityEvent<GameObject>();

    static Settings()
    {
        Load();
    }

    private static void Load()
    {

    }
}