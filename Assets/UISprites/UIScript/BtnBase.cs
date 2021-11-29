using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnBase : MonoBehaviour
{
    private static List<BtnBase> _btns = new List<BtnBase>();
    private static void RaiseOtherButtonClickedEvent(BtnBase callingBtn)
    {
        foreach (BtnBase btn in _btns)
        {
            if (btn != callingBtn)
            {
                btn.OtherButtonClicked(callingBtn);
            }
        }
    }

    protected virtual void Awake()
    {
        _btns.Add(this);
    }
    protected virtual void OnDestroy()
    {
        _btns.Remove(this);
    }

    protected virtual void Clicked()
    {
        RaiseOtherButtonClickedEvent(this);
    }

    protected virtual void OtherButtonClicked(BtnBase clickingBtn)
    {  }
}
