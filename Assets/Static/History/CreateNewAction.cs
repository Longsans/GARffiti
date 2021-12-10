using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewAction : Action
{
    protected Stroke _previousStroke;

    public CreateNewAction(Stroke previousStroke)
    {
        _previousStroke = previousStroke;
    }

    public override void Redo()
    {
    }

    public override void Undo()
    {
        ARCursor.Instance.CurrentStroke = _previousStroke;
    }
}
