using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    private List<ObjectiveInteraction> objectives;
    private List<GeneralInteraction> everythings;

    public void AddObjective(string objective, string intent, int time, string character, float distance)
    {
        ObjectiveInteraction objectiveInteraction = new ObjectiveInteraction(objective, intent, time, character, distance);
        objectives.Add(objectiveInteraction);
    }
}

public class ObjectiveInteraction
{
    public ObjectiveInteraction(string objective, string intent, int time, string character, float distance)
    {
        Objective = objective;
        Intent = intent;
        Time = time;
        Character = character;
        Distance = distance;
    }

    public string Objective;
    public string Intent;
    public int Time;
    public string Character;
    public float Distance;
}

public class GeneralInteraction
{
    public GeneralInteraction(string intent, int time, string character, float distance)
    {
        Intent = intent;
        Time = time;
        Character = character;
        Distance = distance;
    }

    public string Intent;
    public int Time;
    public string Character;
    public float Distance;
}