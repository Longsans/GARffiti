using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : MonoBehaviour
{
    private static File _instance;
    public static File Instance { get => _instance; }

    private List<Stroke> _strokes = new List<Stroke>();
    public List<Stroke> Strokes { get => _strokes; }

    private void Awake()
    {
        _instance = this;
    }

    public static void AddStroke(Stroke stroke)
    {
        if (_instance._strokes.IndexOf(stroke) < 0)
        {
            _instance._strokes.Add(stroke);
            stroke.Show();
            ARCursor.Instance.CurrentStroke = stroke;

            // This is just for grouping gameobjects together to clean up the tree view in scene
            stroke.transform.parent = _instance.gameObject.transform;
        }
    }

    public static void RemoveStroke(Stroke stroke)
    {
        List<Stroke> strokes = _instance._strokes;
        if (_instance._strokes.Remove(stroke))
        {
            stroke.Hide();

            if (strokes.Count > 0)
            {
                ARCursor.Instance.CurrentStroke = strokes[strokes.Count - 1];
            }
            else
                ARCursor.Instance.CurrentStroke = null;
        }
    }

    public static void Clear()
    {
        foreach (Stroke stroke in _instance._strokes)
        {
            stroke.Hide();
        }
        _instance._strokes.Clear();
        ARCursor.Instance.CurrentStroke = null;

        // Add action to history
    }
}
