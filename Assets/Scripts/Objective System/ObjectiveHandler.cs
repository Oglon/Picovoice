using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveHandler : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI TextMeshPro { get; private set; }

    private List<Objective> _objectives;
    private Objective _currentObjective;
    private int index = 1;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Office Level 1")
        {
            _objectives = new List<Objective>
            {
                new Objective("Ask the Intern if he saw someone in your office!", true),
                new Objective("Ask the Boss if he saw someone in your office!", false),
                new Objective("Ask the Colleague if he saw someone in your office!", false),
                new Objective(
                    "Solve the Cesar Cipher \"XCLLQ\" in your Office and present the solution to your Colleague!",
                    false),
                new Objective("Answer the Colleague if you will help the Intern!", false),
                new Objective("Ask if the Intern needs your help!", false),
                new Objective("Solve Gummy Bear Riddle (G,R,Y,W) and present the solution to your Colleague!", false),
                new Objective("Talk to the Boss!", false),
                new Objective("Solve the Cipher (O U ꓶ C) and present the solution to your Colleague!", false),
                new Objective("Talk to the Intern, he knows how to find your file!", false),
                new Objective("Get the Door Code from your Boss!", false),
                new Objective("Confirm your Boss that you want the Door Code!", false),
                new Objective("Tell the Intern the Door Code", false),
                new Objective("Level Complete!", false),
            };

            _currentObjective = _objectives[0];
            TextMeshPro.SetText(_currentObjective.getDescription());
        }
        else
        {
            _objectives = new List<Objective>
            {
                new Objective("Ask the Intern about the secret Price!", true),
                new Objective("Talk to the Boss about the secret price!", false),
                new Objective("Ask your Colleague if you can help him!", false),
                new Objective("Confirm that you will help your Colleague!", false),
                new Objective("Decrypt the code {24,66,53,88} and tell your Colleague the solution!", false),
                new Objective("Ask the Boss if he wanted to talk with you!", false),
                new Objective("Ask the Intern if he needs your help!", false),
                new Objective("Solve the Morse Code (.|...|_._.|._|.__.|.) and present the solution to the Colleague!",
                    false),
                new Objective("Talk to the Boss!", false),
                new Objective("Solve the Element Cipher {45, 53, 102} and present the solution to the Intern!", false),
                new Objective("Talk to Boss!", false),
                new Objective("Level Complete!", false)
            };

            _currentObjective = _objectives[0];
            TextMeshPro.SetText(_currentObjective.getDescription());
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

        TextMeshPro.SetText(_currentObjective.getDescription());
    }

    public int getCurrentIndex()
    {
        return index;
    }
}