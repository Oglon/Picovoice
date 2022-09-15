using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective
{
    private string _objectiveDesc { get; set; }
    private bool _objectiveState { get; set; }
    private string _objectiveHint { get; set; }

    public Objective(string desc, bool state, string hint)
    {
        this._objectiveDesc = desc;
        this._objectiveState = state;
        this._objectiveHint = hint;
    }

    public bool GetState()
    {
        return _objectiveState;
    }

    public void setState(bool state)
    {
        _objectiveState = state;
    }

    public string getDescription()
    {
        return _objectiveDesc;
    }

    public void setDescription(string desc)
    {
        _objectiveDesc = desc;
    }

    public string getHint()
    {
        return _objectiveHint;
    }

    public void setHint(string hint)
    {
        _objectiveHint = hint;
    }
}