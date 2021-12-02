using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBtnScript : MonoBehaviour
{
    public void ClearCanvas()
    {
        ClearAction clearAction = new ClearAction();
        History.AddAction(clearAction);
    }
}
