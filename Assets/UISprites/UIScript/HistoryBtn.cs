using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryBtn : MonoBehaviour
{
    public void Undo()
    {
        History.Undo();
    }

    public void Redo()
    {
        History.Redo();
    }
}
