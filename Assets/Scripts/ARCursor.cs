using UnityEngine;
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
    public GameObject LinePrefab;

    private BaseDrawStrategy _drawStrategy;

    void Start()
    {
        //StrokePrefab.GetComponent<TrailRenderer>().sharedMaterial = Resources.Load<Material>("Materials/Stroke");
    }

    void Update()
    {
        if (DrawMode == __DrawMode.PlanesOnly && _drawStrategy?.GetType() != typeof(PlaneDrawStrategy))
        {
            _drawStrategy?.Dispose();
            _drawStrategy = new PlaneDrawStrategy(this);
        }
        else if (DrawMode == __DrawMode.SpaceOnly && _drawStrategy?.GetType() != typeof(SpaceDrawStrategy))
        {
            _drawStrategy?.Dispose();
            _drawStrategy = new SpaceDrawStrategy(this);
        }

        _drawStrategy.Draw();
    }
}
