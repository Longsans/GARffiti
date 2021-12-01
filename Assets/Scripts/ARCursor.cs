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

    private Stroke _currentStroke;
    public Stroke CurrentStroke { get => _currentStroke; }

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
        _currentStroke?.SetColor(color);
    }

    private void BrushWidthChanged(float width)
    {
        _lineRend.widthMultiplier = _trailRend.widthMultiplier = width;
        _currentStroke?.SetWidth(width);
    }

    private void DrawStratChanged(DrawModeType drawModeType)
    {
        DrawMode = drawModeType;
    }

    private void CreateSharedMaterialForBrush(Stroke stroke)
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

        _currentStroke = stroke;
        _currentStroke.SetMaterial(_lineRend.sharedMaterial);
    }
}
