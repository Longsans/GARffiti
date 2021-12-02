using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAction : Action
{
    private Stroke _stroke;
    public DrawAction(Stroke stroke)
    {
        _stroke = stroke;
        File.AddStroke(stroke);
    }

    public override void Redo()
    {
        File.AddStroke(_stroke);
    }

    public override void Undo()
    {
        File.RemoveStroke(_stroke);
    }

    public override void Remove()
    {
        _stroke.Dispose();
        _stroke = null;
    }
}
