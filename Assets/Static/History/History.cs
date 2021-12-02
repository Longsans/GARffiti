using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class History
{
    private static List<Action> _actions = new List<Action>();
    private static List<Action> _redoActions = new List<Action>();
    public static int MaxActionCount = 10;

    public static void Undo()
    {
        if (_actions.Count == 0)
            return;

        Action currentAction = _actions[_actions.Count - 1];
        currentAction.Undo();
        _actions.RemoveAt(_actions.Count - 1);
        _redoActions.Add(currentAction);
    }
    public static void Redo()
    {
        if (_redoActions.Count == 0)
            return;

        Action currentAction = _redoActions[_redoActions.Count - 1];
        currentAction.Redo();
        _redoActions.RemoveAt(_redoActions.Count - 1);
        _actions.Add(currentAction);
    }

    public static void AddAction(Action action)
    {
        _actions.Add(action);

        foreach (Action redoAction in _redoActions)
        {
            redoAction.Remove();
        }
        _redoActions.Clear();

        if (_actions.Count > MaxActionCount)
        {
            _actions[0].Complete();
            _actions.RemoveAt(0);
        }
    }
}
