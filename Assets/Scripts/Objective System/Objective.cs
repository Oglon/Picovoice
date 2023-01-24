using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class Objective : ScriptableObject
{
    public string objectiveDesc;
    public string objectiveHint;
    public int questIndex;

    public Objective(string desc, bool state, string hint)
    {
        objectiveDesc = desc;
        objectiveHint = hint;
    }

    public string getDescription()
    {
        return objectiveDesc;
    }

    public string getHint()
    {
        return objectiveHint;
    }
}