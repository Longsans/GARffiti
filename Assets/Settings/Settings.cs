using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Settings
{
    private static Color _brushColor;
    public static Color BrushColor 
    {
        get => _brushColor;
        set
        {
            if (_brushColor == value)
                return;

            _brushColor = value;
            if (_lineRend != null)
                _lineRend.sharedMaterial.color = value;

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
            if (_lineRend != null && _trailRend != null)
                _lineRend.widthMultiplier = _trailRend.widthMultiplier = value;

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
            if (_arCursor != null)
                _arCursor.DrawMode = value;

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
            CreateSharedMaterialForBrush();
            onTextureChanged.Invoke(value);
        }
    }

    private static LineRenderer _lineRend;
    private static TrailRenderer _trailRend;
    private static ARCursor _arCursor;
    public static ARCursor ARCursor 
    {
        get => _arCursor;
        set
        {
            _arCursor = value;
            if (_arCursor != null)
            {
                _arCursor.DrawMode = _drawMode;
                _lineRend = _arCursor.LinePrefab.GetComponent<LineRenderer>();
                _trailRend = _arCursor.StrokePrefab.GetComponent<TrailRenderer>();
                CreateSharedMaterialForBrush();
            }
        }
    }

    public static UnityEvent<Texture2D> onTextureChanged = new UnityEvent<Texture2D>();
    public static UnityEvent<float> onBrushWidthChanged = new UnityEvent<float>();
    public static UnityEvent<Color> onBrushColorChanged = new UnityEvent<Color>();
    public static UnityEvent<ARCursor.DrawModeType> onDrawModeChanged = new UnityEvent<ARCursor.DrawModeType>();

    static Settings()
    {
        Load();
    }

    private static void Load()
    {

    }

    private static void CreateSharedMaterialForBrush()
    {
        if (_texture != null)
        {
            _lineRend.sharedMaterial = _trailRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Std. Material"));
            _lineRend.sharedMaterial.mainTexture = _texture;
        }
        else
        {
            _lineRend.sharedMaterial = _trailRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Material"));
        }
    }
}