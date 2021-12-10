using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingAction : Action
{
    private ModelScript _modelScript;

    public PlacingAction(ModelScript modelScript) : base()
    {
        _modelScript = modelScript;
        File.AddModel(modelScript);
    }

    public override void Redo()
    {
        File.AddModel(_modelScript);
    }

    public override void Undo()
    {
        File.RemoveModel(_modelScript);
    }

    public override void Remove()
    {
        _modelScript.Destroy();
        _modelScript = null;
    }
}
