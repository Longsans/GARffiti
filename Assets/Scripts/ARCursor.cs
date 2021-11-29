using UnityEngine;
using Assets.Scripts;

public class ARCursor : MonoBehaviour
{
    public enum DrawModeType
    {
        PlanesOnly,
        SpaceOnly,
    }

    private DrawModeType _drawMode;
    public DrawModeType DrawMode 
    {
        get => _drawMode;
        set
        {
            if (_drawMode != value)
            {
                _drawMode = value;
                CreateDrawStrat();
            }
        }
    }

    public GameObject StrokePrefab;
    public GameObject LinePrefab;

    public Material CurrentSharedMaterial { get => _lineRend.sharedMaterial; }

    private BaseDrawStrategy _drawStrategy = null;

    private LineRenderer _lineRend;
    private TrailRenderer _trailRend;

    private LineRenderer _currentStrokeLineRenderer;
    private TrailRenderer _currentStrokeTrailRenderer;

    void Awake()
    {
        _lineRend = LinePrefab.GetComponent<LineRenderer>();
        _trailRend = StrokePrefab.GetComponent<TrailRenderer>();
    }

    void Update()
    {
        _drawStrategy.Draw();
    }

    private void OnEnable()
    {
        if (_lineRend.sharedMaterial != null)
            _lineRend.sharedMaterial.color = Settings.BrushColor;

        _lineRend.widthMultiplier = _trailRend.widthMultiplier = Settings.BrushWidth;

        DrawMode = Settings.DrawMode;
        // In case the default material is the same as the setting at the start of the program it can be null
        if (_drawStrategy == null)
            CreateDrawStrat();

        Settings.onBrushColorChanged.AddListener(BrushColorChanged);
        Settings.onBrushWidthChanged.AddListener(BrushWidthChanged);
        Settings.onDrawModeChanged.AddListener(DrawStratChanged);
    }
    private void OnDisable()
    {
        Settings.onBrushColorChanged.RemoveListener(BrushColorChanged);
        Settings.onBrushWidthChanged.RemoveListener(BrushWidthChanged);
        Settings.onDrawModeChanged.RemoveListener(DrawStratChanged);
    }

    private void CreateDrawStrat()
    {
        if (_drawStrategy != null)
        {
            _drawStrategy.Dispose();
            _drawStrategy.DrawPhaseStarted.RemoveListener(CreateSharedMaterialForBrush);
        }

        if (DrawMode == DrawModeType.PlanesOnly)
        {
            _drawStrategy = new PlaneDrawStrategy(this);
        }
        else if (DrawMode == DrawModeType.SpaceOnly)
        {
            _drawStrategy = new SpaceDrawStrategy(this);
        }
        _drawStrategy.DrawPhaseStarted.AddListener(CreateSharedMaterialForBrush);
    }

    private void BrushColorChanged(Color color)
    {
        if (_lineRend.sharedMaterial != null)
            _lineRend.sharedMaterial.color = color;
    }

    private void BrushWidthChanged(float width)
    {
        _lineRend.widthMultiplier = _trailRend.widthMultiplier = width;

        if (_currentStrokeLineRenderer != null)
        {
            _currentStrokeLineRenderer.widthMultiplier = width;
        }

        if (_currentStrokeTrailRenderer != null)
        {
            _currentStrokeTrailRenderer.widthMultiplier = width;
        }
    }

    private void DrawStratChanged(DrawModeType drawModeType)
    {
        DrawMode = drawModeType;
    }

    private void CreateSharedMaterialForBrush(GameObject stroke)
    {
        // This is to create new material for the prefabs so that the new brush doesn't share resource with the old one
        if (Settings.Texture)
        {
            _lineRend.sharedMaterial = _trailRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Std. Material"));
            _lineRend.sharedMaterial.mainTexture = Settings.Texture;
        }
        else
        {
            _lineRend.sharedMaterial = _trailRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Material"));
            _lineRend.sharedMaterial.color = Settings.BrushColor;
        }

        // Set the newly created strok mat and store it to set line width dynamically later
        _currentStrokeLineRenderer = null;
        _currentStrokeTrailRenderer = null;

        if (DrawMode == DrawModeType.PlanesOnly)
        {
            _currentStrokeLineRenderer = stroke.GetComponent<LineRenderer>();
            _currentStrokeLineRenderer.material = CurrentSharedMaterial;
        }
        else
        {
            _currentStrokeTrailRenderer = stroke.GetComponent<TrailRenderer>();
            _currentStrokeTrailRenderer.material = CurrentSharedMaterial;
        }
    }
}
