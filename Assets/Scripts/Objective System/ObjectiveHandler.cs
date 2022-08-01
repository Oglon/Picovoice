using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    private List<Objective> _objectives;
    private Objective _currentObjective;
    private int index = 1;


    void Start()
    {
        _objectives = new List<Objective>
        {
            new Objective("Talk to the Intern", true),
            new Objective("Talk to the Boss", false),
            new Objective("Talk to the Colleague", false),
            new Objective("Solve Cesar Cipher and present the solution to your Colleague", false),
            new Objective("Talk to Intern", false),
            new Objective("Solve Gummy Bear Riddle and present the solution to your Colleague", false),
            new Objective("Talk to the Boss", false),
            new Objective("Solve the Cipher and present the solution to your Colleague", false),
            new Objective("Talk to the Intern", false),
            new Objective("Get the Door Code from your Boss", false),
            new Objective("Tell the Intern about the Code", false),
            new Objective("Get your file from the cleaning cabinet", false),
            new Objective("Level Complete", false)
        };

        _currentObjective = _objectives[0];
        Debug.Log(_currentObjective.getDescription());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Progress();
        }
    }

    private void Progress()
    {
        if (index == _objectives.Count)
            return;

        _currentObjective.setState(false);
        _currentObjective = _objectives[index];
        _currentObjective.setState(true);
        index++;

        Debug.Log(_currentObjective.getDescription());
    }
}