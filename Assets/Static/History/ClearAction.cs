using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAction : CreateNewAction
{
    IEnumerable<Stroke> _strokes;
    IEnumerable<ModelScript> _modelScripts;
    

    public ClearAction(IEnumerable<Stroke> strokes, IEnumerable<ModelScript> modelScripts, Stroke previousStroke) : base(previousStroke)
    {
        _strokes = strokes;
        _modelScripts = modelScripts;
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

        foreach (ModelScript modelScript in _modelScripts)
        {
            File.AddModel(modelScript);
        }

        ARCursor.Instance.CurrentStroke = _previousStroke;
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
