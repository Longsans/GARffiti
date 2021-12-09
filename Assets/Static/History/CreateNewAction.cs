using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewAction : Action
{
    protected ModelScript _previousModelScript;
    protected Stroke _previousStroke;

    public CreateNewAction(ModelScript previousScript, Stroke previousStroke)
    {
        _previousStroke = previousStroke;
        _previousModelScript = previousScript;
    }

    public override void Redo()
    {
    }

    public override void Undo()
    {
        ARCursor.Instance.CurrentModelScript = _previousModelScript;
        ARCursor.Instance.CurrentStroke = _previousStroke;
    }
}
