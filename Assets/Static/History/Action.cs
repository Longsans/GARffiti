using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public abstract void Undo();
    public abstract void Redo();

    // This is called when the action is undo-ed and another action is inserted to the history list
    public virtual void Remove() { }

    // This is called when the history remove it and it is in the not undo-ed state
    public virtual void Complete() { }
}
