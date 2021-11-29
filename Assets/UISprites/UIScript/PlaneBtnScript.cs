using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneBtnScript : BtnBase
{
    public Image Icon;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Clicked()
    {
        base.Clicked();

        Settings.DrawMode = Settings.DrawMode == ARCursor.DrawModeType.PlanesOnly ? ARCursor.DrawModeType.SpaceOnly : ARCursor.DrawModeType.PlanesOnly;
        SetToCorrectIcon();
    }

    private void OnEnable()
    {
        SetToCorrectIcon();
    }

    private void SetToCorrectIcon()
    {
        if (Settings.DrawMode == ARCursor.DrawModeType.SpaceOnly)
        {
            Icon.sprite = Resources.Load<Sprite>("Icons/3d-cube-white");
        }
        else
        {
            Icon.sprite = Resources.Load<Sprite>("Icons/white-trap");
        }
    }
}
