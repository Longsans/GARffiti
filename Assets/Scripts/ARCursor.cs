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

    private BaseDrawStrategy _drawStrategy;

    void Awake()
    {
        Settings.ARCursor = this;
        if (_drawStrategy == null)
            CreateDrawStrat();
    }

    void Update()
    {
        _drawStrategy.Draw();
    }

    private void CreateDrawStrat()
    {
        _drawStrategy?.Dispose();
        if (DrawMode == DrawModeType.PlanesOnly)
        {
            _drawStrategy = new PlaneDrawStrategy(this);
        }
        else if (DrawMode == DrawModeType.SpaceOnly)
        {
            _drawStrategy = new SpaceDrawStrategy(this);
        }
    }
}
