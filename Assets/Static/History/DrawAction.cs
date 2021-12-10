using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAction : CreateNewAction
{
    private Stroke _stroke;
    public DrawAction(Stroke stroke, ModelScript previousScript, Stroke previousStroke) : base(previousScript, previousStroke)
    {
        _stroke = stroke;
        File.AddStroke(stroke);
    }

    public override void Redo()
    {
        File.AddStroke(_stroke);
        ARCursor.Instance.CurrentStroke = _stroke;
    }

    public override void Undo()
    {
        File.RemoveStroke(_stroke);
        base.Undo();
    }

    public override void Remove()
    {
        _stroke.Dispose();
        _stroke = null;
    }
}
