using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public abstract void Undo();
    public abstract void Redo();
    public virtual void Complete()
    {
        History.AddAction(this);
    }
}
