using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStroke : Stroke
{
    public SpaceStroke(GameObject brushObject) : base(brushObject)
    {
        LineRenderer.alignment = LineAlignment.View;
    }
}
