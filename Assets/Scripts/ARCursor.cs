using UnityEngine;
using Assets.Scripts;

public class ARCursor : MonoBehaviour
{
    public static ARCursor Instance { get => _instance; }
    private static ARCursor _instance;

    public Camera ARCam;

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
    public Material CurrentSharedMaterial { get => _lineRend.sharedMaterial; }

    private BaseDrawStrategy _drawStrategy = null;
    public BaseDrawStrategy DrawStrategy { get => _drawStrategy; }

    private LineRenderer _lineRend;

    private Stroke _currentStroke;
    public Stroke CurrentStroke { 
        get => _currentStroke;
        set
        {
            _currentStroke = value;
        }
    }

    void Awake()
    {
        _lineRend = StrokePrefab.GetComponent<LineRenderer>();
        _instance = this;
    }

    private void OnEnable()
    {
        if (_lineRend.sharedMaterial != null)
            _lineRend.sharedMaterial.color = Settings.BrushColor;

        _lineRend.widthMultiplier = Settings.BrushWidth;

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
            _drawStrategy.PlacingPhaseStarted.RemoveListener(PlacingPhaseStarted);
        }

        if (DrawMode == DrawModeType.PlanesOnly)
        {
            _drawStrategy = new PlaneDrawStrategy(this);
        }
        else if (DrawMode == DrawModeType.SpaceOnly)
        {
            _drawStrategy = new SpaceDrawStrategy(this);
        }
        _drawStrategy.ARCam = ARCam;

        _drawStrategy.DrawPhaseStarted.AddListener(CreateSharedMaterialForBrush);
        _drawStrategy.PlacingPhaseStarted.AddListener(PlacingPhaseStarted);
    }

    private void BrushColorChanged(Color color)
    {
        _currentStroke?.SetColor(color);
    }

    private void BrushWidthChanged(float width)
    {
        _lineRend.widthMultiplier = width;
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
            _lineRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Std. Material"));
            _lineRend.sharedMaterial.mainTexture = Settings.Texture;
        }
        else
        {
            _lineRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Material"));
            _lineRend.sharedMaterial.color = Settings.BrushColor;
        }
        stroke.SetMaterial(_lineRend.sharedMaterial);
    }

    private void PlacingPhaseStarted(ModelScript modelScript)
    {
    }
}
