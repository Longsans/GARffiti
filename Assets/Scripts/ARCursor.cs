using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Assets.Scripts;

public class ARCursor : MonoBehaviour
{
    public enum __DrawMode
    {
        PlanesOnly,
        SpaceOnly,
    }

    public __DrawMode DrawMode;
    public GameObject StrokePrefab;

    private BaseDrawStrategy _drawStrategy;

    void Start()
    {
        switch (DrawMode)
        {
            case __DrawMode.PlanesOnly:
                _drawStrategy = new PlanesDrawStrategy(this);
                break;
            case __DrawMode.SpaceOnly:
                _drawStrategy = new SpaceDrawStrategy(this);
                break;
        }
    }

    void Update()
    {
        _drawStrategy.UpdateCursorPosition();
        _drawStrategy.Draw();
    }
}
