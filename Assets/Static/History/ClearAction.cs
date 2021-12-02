using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAction : Action
{
    IEnumerable<Stroke> _strokes;
    public ClearAction()
    {
        _strokes = File.Instance.Strokes.ToArray();
        File.Clear();
    }

    public override void Redo()
    {
        File.Clear();
    }

    public override void Undo()
    {
        foreach (Stroke stroke in _strokes)
        {
            File.AddStroke(stroke);
        }
    }

    public override void Complete()
    {
        foreach (Stroke stroke in _strokes)
        {
            stroke.Dispose();
        }
        _strokes = null;
    }
}
