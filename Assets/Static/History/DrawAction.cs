using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAction : Action
{
    public GameObject Brush;
    public List<Vector2> Positions = new List<Vector2>();
    public float StepDelta = 0.1f;

    public override void Redo()
    {
        Brush.SetActive(true);
    }

    public override void Undo()
    {
        Brush.SetActive(false);
        TrailRenderer trailRenderer = new TrailRenderer();
    }
}
