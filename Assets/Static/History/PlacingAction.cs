using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingAction : CreateNewAction
{
    private ModelScript _modelScript;

    public PlacingAction(ModelScript modelScript, ModelScript previousScript, Stroke previousStroke) : base(previousScript, previousStroke)
    {
        _modelScript = modelScript;
        File.AddModel(modelScript);
    }

    public override void Redo()
    {
        File.AddModel(_modelScript);
        ARCursor.Instance.CurrentModelScript = _modelScript;
    }

    public override void Undo()
    {
        File.RemoveModel(_modelScript);
        base.Undo();
    }

    public override void Remove()
    {
        _modelScript.Destory();
        _modelScript = null;
    }
}
