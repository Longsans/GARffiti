using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocateAction : Action
{
    private ModelScript _model;
    private Vector3 _lastLocation;
    private Vector3 _currentLocation;

    public RelocateAction(ModelScript model, Vector3 lastLocation, Vector3 currentLocation)
    {
        _model = model;
        _lastLocation = lastLocation;
        _currentLocation = currentLocation;
    }

    public override void Redo()
    {
        _model.MoveTo(_currentLocation);
    }

    public override void Undo()
    {
        _model.MoveTo(_lastLocation);
    }
}
