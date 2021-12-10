using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EditModelAction : Action
{
    public ModelScript model;
    public float lastScale;
    public float currentScale;
    public Vector3 lastRot;
    public Vector3 currentRot;

    public EditModelAction(ModelScript m, float lScale, float cScale, Vector3 lRot, Vector3 cRot)
    {
        model = m;
        lastScale = lScale;
        currentScale = cScale;
        lastRot = lRot;
        currentRot = cRot;
    }

    public override void Undo()
    {
        model.EditPanel.GetComponent<EditModelPanelUI>().UpdateState(lastScale, lastRot.x, lastRot.y, lastRot.z);
    }

    public override void Redo()
    {
        model.EditPanel.GetComponent<EditModelPanelUI>().UpdateState(currentScale, currentRot.x, currentRot.y, currentRot.z);
    }
}
