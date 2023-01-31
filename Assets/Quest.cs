using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public event Action OnFollowingQuest;
    public event Action OnProgress;

    private Analytics _analytics;

    public string questName;

    public List<Objective> objectives;

    public Objective currentObjective;

    public Quest followUp;

    public int questIndex;

    public Objective GetCurrentObjective()
    {
        return currentObjective;
    }

    public Quest GetFollowUp()
    {
        return followUp;
    }

    public void Progress()
    {
        _analytics = GameObject.Find("Analytics").GetComponent<Analytics>();
        _analytics.AddObjective(currentObjective.ToString());
        if ((objectives.IndexOf(currentObjective) + 1).Equals(objectives.Count))
        {
            OnFollowingQuest?.Invoke();
        }
        else
        {
            // currentObjective = objectives[objectives.IndexOf(currentObjective)+1];
            OnProgress?.Invoke();
        }
    }
}