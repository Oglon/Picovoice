using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveHandler : MonoBehaviour
{
    public TextMeshProUGUI ObjectiveTMP;
    public TextMeshProUGUI HintTMP;
    public GameObject HintPanel;
    public SpriteRenderer Icon;

    public TextMeshProUGUI StartTMP;
    public GameObject StartPanel;

    [field: SerializeField] public GameObject Slider { get; private set; }

    private List<Objective> _objectives;
    private Objective _currentObjective;
    private int index = 1;

    private float counter = 7f;
    private bool skip = false;

    private CharacterController _characterController;

    public string startText;

    void Start()
    {
        ObjectiveTMP = GameObject.Find("Objective").GetComponent<TextMeshProUGUI>();
        HintTMP = GameObject.Find("Hint").GetComponent<TextMeshProUGUI>();
        HintPanel = GameObject.Find("Hint Box");

        StartTMP = GameObject.Find("StartText").GetComponent<TextMeshProUGUI>();
        StartPanel = GameObject.Find("StartBox");

        Icon = GameObject.Find("Icon").GetComponent<SpriteRenderer>();

        _characterController = GameObject.Find("PlayerCapsule").GetComponent<CharacterController>();

        _characterController.enabled = false;

        StartPanel.SetActive(true);
        StartTMP.gameObject.SetActive(true);

        Slider = GameObject.Find("Slider");
        Slider.SetActive(false);

        if (SceneManager.GetActiveScene().name.Contains("1"))
        {
            Icon.sprite = Resources.Load<Sprite>("File");
            startText =
                "My file is missing from my desk. Someone took it while I was gone. I should ask the Intern in front of my office if he saw someone in my office!";
        }
        else
        {
            Icon.sprite = Resources.Load<Sprite>("Price");
            startText =
                "There are talks around the office about a secret price. I should ask the Intern in front of my office if he knows something about that.";
        }

        StartTMP.text = startText;

        Icon.gameObject.SetActive(false);

        HintPanel.SetActive(false);
        HintTMP.gameObject.SetActive(false);

        string _sceneName = SceneManager.GetActiveScene().name;
        if (_sceneName.Contains("Office Level 1"))
        {
            _objectives = new List<Objective>
            {
                new Objective("Ask the Intern if he saw someone in your office!", true,
                    "Try asking the Intern 'Did you see someone in my office?'"),
                new Objective("Ask the Boss if he saw someone in your office!", false,
                    "Try asking the Boss 'Did you see someone in my office?'"),
                new Objective("Ask the Colleague if he saw someone in your office!", false,
                    "Try asking your Colleague 'Did you see someone in my office?'"),
                new Objective(
                    "Solve the Cesar Cipher in your Office and present the solution to your Colleague!",
                    false, "The Cipher starts with A"),
                new Objective("Answer the Colleague if you will help the Intern!", false, "Answer with Yes"),
                new Objective("Ask if the Intern needs your help!", false,
                    "Ask the Intern 'Do you need help with the Cipher?'"),
                new Objective("Solve Gummy Bear Riddle and present the solution to your Colleague!", false,
                    "Tell the Colleague the solution 5644"),
                new Objective("Talk to the Boss!", false, "Ask the Boss 'You wanted to talk with me?'"),
                new Objective("Solve the Cipher and present the solution to your Colleague!", false,
                    "Present your Colleague the solution 5276"),
                new Objective("Talk to the Intern, he knows how to find your file!", false,
                    "Ask the Intern if he saw your file."),
                new Objective("Confirm that you will get the door key from the boss", false,
                    "Say yes to your Colleague"),
                new Objective("Get the Door Code from your Boss!", false, "Approach your Boss"),
                new Objective("Confirm your Boss that you want the Door Code!", false, "Respond with Yes"),
                new Objective("Tell the Intern the Door Code", false, "Tell the Intern that the Door Code is 1111"),
                new Objective("Level Complete!", false, "You completed the Level"),
            };

            _currentObjective = _objectives[0];
            ObjectiveTMP.SetText(_currentObjective.getDescription());
            HintTMP.SetText(_currentObjective.getHint());
        }
        else
        {
            _objectives = new List<Objective>
            {
                new Objective("Ask the Intern about the secret Price!", true,
                    "Ask the Intern 'Do you know something about the secret prize?'"),
                new Objective("Talk to the Boss about the secret price!", false,
                    "Ask the Boss 'Do you know about the secret price?'"),
                new Objective("Ask your Colleague if you can help her!", false, "Ask the Colleague if she needs help."),
                new Objective("Confirm that you will help your Colleague!", false, "Respond with Yes"),
                new Objective("Decrypt the code and tell your Colleague the solution!", false,
                    "The code is 5276"),
                new Objective("Ask the Boss if he wanted to talk with you!", false,
                    "Ask the Boss 'You wanted to talk with me?'"),
                new Objective("Ask the Intern if he needs your help!", false, "Ask the Intern 'Do you need my help?'"),
                new Objective("Solve the Morse Code and present the solution to the Colleague!",
                    false, "The morse code is Escape"),
                new Objective("Talk to the Boss!", false, "Ask the Boss if he wanted to talk with you."),
                new Objective("Solve the Element Cipher and present the solution to the Intern!", false,
                    "Look for the corresponding elements, the corresponding letter for 45 would be R"),
                new Objective("Talk to Boss!", false, "Ask your Boss if he wanted to talk with you."),
                new Objective("Level Complete!", false, "You completed the Level")
            };

            _currentObjective = _objectives[0];
            ObjectiveTMP.SetText(_currentObjective.getDescription());
            HintTMP.SetText(_currentObjective.getHint());
        }
    }

    private void Update()
    {
        if (!skip)
        {
            if (counter >= 0)
            {
                counter -= Time.deltaTime;
            }
            else
            {
                _characterController.enabled = true;
                StartPanel.gameObject.SetActive(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Progress();
        }

        if (Input.GetKey(KeyCode.H))
        {
            HintPanel.SetActive(true);
            HintTMP.gameObject.SetActive(true);
        }
        else if (!Input.GetKey(KeyCode.H))
        {
            HintPanel.SetActive(false);
            HintTMP.gameObject.SetActive(false);
        }

        if (index == _objectives.Count)
        {
            Icon.gameObject.SetActive(true);
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

        ObjectiveTMP.SetText(_currentObjective.getDescription());
        HintTMP.SetText(_currentObjective.getHint());
    }

    public int getCurrentIndex()
    {
        return index;
    }
}