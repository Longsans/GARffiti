using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBtnScript : MonoBehaviour
{
    public void ClearCanvas()
    {
        ClearAction clearAction = new ClearAction(
            File.Instance.Strokes.ToArray(), 
            File.Instance.ModelScripts.ToArray(),
            ARCursor.Instance.CurrentStroke
            );
        History.AddAction(clearAction);
    }
}
