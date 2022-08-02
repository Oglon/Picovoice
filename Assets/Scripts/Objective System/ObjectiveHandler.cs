using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveHandler : MonoBehaviour
{
    private List<Objective> _objectives;
    private Objective _currentObjective;
    private int index = 1;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Office Level 1")
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
        else
        {
            _objectives = new List<Objective>
            {
                new Objective("Talk to the Intern", true),
                new Objective("Talk to the Boss about the secret price", false),
                new Objective("Talk to the Colleague", false),
                new Objective("Encode the letter", false),
                new Objective("Talk to Colleague", false),
                new Objective("Talk to Boss", false),
                new Objective("Talk to the Intern", false),
                new Objective("Solve Morse Code", false),
                new Objective("Talk to the Intern", false),
                new Objective("Talk to the Boss", false),
                new Objective("Solve Rhino Code", false),
                new Objective("Talk to Boss", false),
                new Objective("Level Complete", false)
            };

            _currentObjective = _objectives[0];
            Debug.Log(_currentObjective.getDescription()); 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Progress();
        }
    }

    public void Progress()
    {
        if (index == _objectives.Count)
            return;

        _currentObjective.setState(false);
        _currentObjective = _objectives[index];
        _currentObjective.setState(true);
        index++;

        Debug.Log(_currentObjective.getDescription());
    }

    public int getCurrentIndex()
    {
        return index;
    }
}