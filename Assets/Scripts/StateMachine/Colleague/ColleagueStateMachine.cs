using System;
using Pv.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColleagueStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; private set; }

    [field: SerializeField] public TextMeshProUGUI TextMeshPro { get; private set; }
    [field: SerializeField] public bool _sensitive { get; private set; }

    public Camera playerHead { get; private set; }

    AudioClip _clipRecord = null;
    int _sampleWindow = 128;

    public bool _isProcessing;
    public RhinoManager _rhinoManager;

    public static float MicLoudness;
    private string _device;

    private bool _tooLoud;

    private float _delta = 5f;

    private const string
        ACCESS_KEY =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

    private void Start()
    {
        _rhinoManager = RhinoManager.Create(ACCESS_KEY, GetContextPath(), inferenceCallback);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHead = Camera.main;

        TextMeshPro.SetText("");

        SwitchState(new ColleagueWorkingState(this));

        if (SceneManager.GetActiveScene().name.Contains("Sensitive"))
        {
            _sensitive = true;
        }
        else
        {
            _sensitive = false;
        }

        Debug.Log(_sensitive);
    }

    void inferenceCallback(Inference inference)
    {
        if (!IsInTalkingRange())
            return;
        string _sceneName = SceneManager.GetActiveScene().name;
        if (_sceneName.Contains("Office Level 1"))
        {
            if (inference.IsUnderstood)
            {
                _delta = 6;

                MicLoudness = LevelMax();
                Debug.Log(MicLoudness);

                if (MicLoudness > 6)
                {
                    TextMeshPro.SetText("Don't scream at me!");
                    return;
                }

                if (inference.Intent == "Friendly_SomeoneInMyOffice")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Good morning. I didn't see anyone. Maybe the Boss saw someone?");
                            }
                            else
                            {
                                TextMeshPro.SetText("No but maybe the Boss saw something");
                            }

                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Sadly, I did not. Maybe your Colleague saw someone?");
                            }
                            else
                            {
                                TextMeshPro.SetText("Your Colleague may have seen something.");
                            }

                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Well I might have seen something but cou");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                            }

                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "SomeoneInMyOffice")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            TextMeshPro.SetText("No but maybe the Boss saw something");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            TextMeshPro.SetText("Your Colleague may have seen something.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            TextMeshPro.SetText(
                                "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_SomeoneInMyOffice")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Don't talk to me like that");
                            }
                            else
                            {
                                TextMeshPro.SetText("No but maybe the Boss saw something");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Don't talk to me like that");
                            }
                            else
                            {
                                TextMeshPro.SetText("Your Colleague may have seen something.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Don't talk to me like that.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_1111")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 13)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "The file is in the cleaning cabinet.");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "The file is in the cleaning cabinet.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "1111")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 13)
                        {
                            TextMeshPro.SetText(
                                "The file is in the cleaning cabinet.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_1111")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 13)
                        {
                            TextMeshPro.SetText(
                                "The file is in the cleaning cabinet.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_5276")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            TextMeshPro.SetText(
                                "Amazing. The Intern may have an idea on how to get your file.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "5276")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            TextMeshPro.SetText(
                                "Amazing. The Intern may have an idea on how to get your file.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_5276")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            TextMeshPro.SetText(
                                "Amazing. The Intern may have an idea on how to get your file.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_Afoot")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Thank you su much, could you maybe help the Intern as well?");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                            }

                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Afoot")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            TextMeshPro.SetText(
                                "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_Afoot")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Don't talk to me like that.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_Yes")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Amazing, thank you so much!.");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText("Great, he will tell you the rest.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText("Amazing.");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText("Great.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 12)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Of course, the code is 1111.");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText("The code is 1111.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Yes")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            TextMeshPro.SetText(
                                " Great, he will tell you the rest.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            TextMeshPro.SetText(
                                "Great.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 12)
                        {
                            TextMeshPro.SetText(
                                "The code is 1111.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_Yes")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Don't talk to me like that!.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    " Great, he will tell you the rest.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Don't talk to me like that!.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Great.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 12)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Don't talk to me like that!.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "The Code is 1111.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_NeedHelp")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Yes. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "NeedHelp")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            TextMeshPro.SetText(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_NeedHelp")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Please don't talk to me like that.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_GummyBearSolution")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Great. The Boss wanted to talk to you.");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "The Boss wanted to talk to you.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "GummyBearSolution")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            TextMeshPro.SetText(
                                "The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_GummyBearSolution")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Don't talk to me like that.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "The Boss wanted to talk to you.");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_WantedToTalkToMe")
                {
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "We got a new cipher that contains a code. Can you please solve it and present it to your Colleague?");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "WantedToTalkToMe")
                {
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            TextMeshPro.SetText(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_WantedToTalkToMe")
                {
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Don't talk to me like that!");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_InternKnowsAboutFile")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you please get it from the Boss?");
                                ObjectiveHandler.Progress();
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "InternKnowsAboutFile")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            TextMeshPro.SetText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_InternKnowsAboutFile")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            if (_sensitive)
                            {
                                TextMeshPro.SetText(
                                    "Don't talk to me like that.");
                            }
                            else
                            {
                                TextMeshPro.SetText(
                                    "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                                ObjectiveHandler.Progress();
                            }

                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Weather")
                {
                    TextMeshPro.SetText(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "Game")
                {
                    TextMeshPro.SetText(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "HowIsItGoing")
                {
                    TextMeshPro.SetText(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "Travel")
                {
                    TextMeshPro.SetText(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "Name")
                {
                    TextMeshPro.SetText(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
            }
            else
            {
                TextMeshPro.SetText("I'm sorry but I have no idea what you are talking about.");
                ToggleProcessing();
                return;
            }
        }
        else
        {
            if (inference.IsUnderstood)
            {
                _delta = 6;

                MicLoudness = LevelMax();
                Debug.Log(MicLoudness);

                if (MicLoudness > 6)
                {
                    TextMeshPro.SetText("Don't scream at me!");
                    return;
                }

                if (inference.Intent == "Friendly_SecretPrice")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            Debug.Log("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            Debug.Log(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "SecretPrice")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            Debug.Log("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            Debug.Log(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_SecretPrice")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            Debug.Log("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            Debug.Log(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_NeedHelp")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            Debug.Log("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "NeedHelp")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            Debug.Log("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_NeedHelp")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            Debug.Log("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_Yes")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Yes")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_Yes")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_5276")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            Debug.Log("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "5276")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            Debug.Log("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_5276")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            Debug.Log("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_WantedToTalkToMe")
                {
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            Debug.Log("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            Debug.Log(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            Debug.Log(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "WantedToTalkToMe")
                {
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            Debug.Log("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            Debug.Log(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            Debug.Log(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_WantedToTalkToMe")
                {
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            Debug.Log("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            Debug.Log(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            Debug.Log(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_Escape")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            Debug.Log("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Escape")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            Debug.Log("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_Escape")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            Debug.Log("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Friendly_Rhino")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            Debug.Log("I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Rhino")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            Debug.Log("I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Unfriendly_Rhino")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            Debug.Log("I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Weather")
                {
                    Debug.Log(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "Game")
                {
                    Debug.Log(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "HowIsItGoing")
                {
                    Debug.Log(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "Travel")
                {
                    Debug.Log(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
                else if (inference.Intent == "Name")
                {
                    Debug.Log(
                        "The weather is great");
                    ToggleProcessing();
                    return;
                }
            }
            else
            {
                Debug.Log("Didn't understand the command.\n");
                ToggleProcessing();
                return;
            }
        }

        _isProcessing = false;
    }

    private static string GetContextPath()
    {
        return "E:/UnityProjects/Picovoice/Assets/StreamingAssets/contexts/windows/colleague_windows.rhn";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerListenRange);
    }

    protected bool IsInTalkingRange()
    {
        float playerDistanceSqr =
            (Player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= PlayerListenRange * PlayerListenRange;
    }

    private void ToggleProcessing()
    {
        _isProcessing = !_isProcessing;

        if (!_isProcessing)
        {
            SwitchState(new ColleagueWorkingState(this));
        }
    }

    public void StartProcessing()
    {
        if (_isProcessing)
            return;

        ToggleProcessing();
        _rhinoManager.Process();
    }

    public void Subtitles()
    {
        if (_delta > 0)
        {
            _delta -= Time.deltaTime;
        }
    }

    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        return levelMax;
    }

    bool _isInitialized;

// start mic when scene starts
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

//stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


// make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }
        }

        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }
}