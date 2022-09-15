using Febucci.UI;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColleagueStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; private set; }
    [field: SerializeField] public AudioLoudnessDetection Loudness { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer TextAnimatorPlayer { get; private set; }
    [field: SerializeField] public GameObject SubtitlePanel { get; private set; }
    [field: SerializeField] public bool Sensitive { get; private set; }
    public Camera PlayerHead { get; private set; }

    private static float _delta;

    public bool isProcessing;
    private RhinoManager _rhinoManager;

    private const string
        AccessKey =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

    private void Start()
    {
        _rhinoManager = RhinoManager.Create(AccessKey, GetContextPath(), InferenceCallback);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHead = Camera.main;

        TextAnimatorPlayer.ShowText("");

        SwitchState(new ColleagueWorkingState(this));

        Sensitive = SceneManager.GetActiveScene().name.Contains("Sensitive");
        // Loudness.gameObject.SetActive(false);
    }

    void InferenceCallback(Inference inference)
    {
        if (!IsInTalkingRange())
            return;
        // Loudness.gameObject.SetActive(true);
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Office Level 1"))
        {
            if (OfficeLevel1(inference)) return;
        }
        else
        {
            if (OfficeLevel2(inference)) return;
        }

        // Loudness.gameObject.SetActive(false);

        isProcessing = false;
    }

    private bool OfficeLevel1(Inference inference)
    {
        _delta = 10f;
        SubtitlePanel.SetActive(true);

        if (inference.IsUnderstood)
        {
            // if (MicLoudness > 6)
            // {
            //     TextAnimatorPlayer.ShowText("Don't scream at me!");
            //     return;
            // }

            if (inference.Intent == "Friendly_SomeoneInMyOffice")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        TextAnimatorPlayer.ShowText(
                            Sensitive
                                ? "Good morning. I didn't see anyone. Maybe the Boss saw someone?"
                                : "No but maybe the Boss saw something");

                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        TextAnimatorPlayer.ShowText(Sensitive
                            ? "Sadly, I did not. Maybe your Colleague saw someone?"
                            : "Your Colleague may have seen something.");

                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        TextAnimatorPlayer.ShowText(
                            Sensitive
                                ? "Well I might have seen something but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?"
                                : "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");

                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "SomeoneInMyOffice")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        TextAnimatorPlayer.ShowText("No but maybe the Boss saw something");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        TextAnimatorPlayer.ShowText("Your Colleague may have seen something.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_SomeoneInMyOffice")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText("Don't talk to me like that");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText("No but maybe the Boss saw something");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText("Don't talk to me like that");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText("Your Colleague may have seen something.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText("Don't talk to me like that.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_1111")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "The file is in the cleaning cabinet.");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "The file is in the cleaning cabinet.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "1111")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        TextAnimatorPlayer.ShowText(
                            "The file is in the cleaning cabinet.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_1111")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        TextAnimatorPlayer.ShowText(
                            "The file is in the cleaning cabinet.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_5276")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Amazing. The Intern may have an idea on how to get your file.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "5276")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Amazing. The Intern may have an idea on how to get your file.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_5276")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Amazing. The Intern may have an idea on how to get your file.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_Afoot")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        TextAnimatorPlayer.ShowText(
                            Sensitive
                                ? "Thank you su much, could you maybe help the Intern as well?"
                                : "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");

                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Afoot")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        TextAnimatorPlayer.ShowText(
                            "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_Afoot")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText("Don't talk to me like that.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_Yes")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 5)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText("Amazing, thank you so much!.");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText("Great, he will tell you the rest.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText("Amazing.");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText("Great.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Of course, the code is 1111.");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText("The code is 1111.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Yes")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 5)
                    {
                        TextAnimatorPlayer.ShowText(
                            " Great, he will tell you the rest.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Great.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        TextAnimatorPlayer.ShowText(
                            "The code is 1111.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_Yes")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 5)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                " Great, he will tell you the rest.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "Great.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }

                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "The Code is 1111.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_NeedHelp")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "NeedHelp")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_NeedHelp")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Please don't talk to me like that.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_GummyBearSolution")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Great. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "GummyBearSolution")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        TextAnimatorPlayer.ShowText(
                            "The Boss wanted to talk to you.");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_GummyBearSolution")
            {
                if (this.gameObject.CompareTag($"Colleague"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_WantedToTalkToMe")
            {
                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "We got a new cipher that contains a code. Can you please solve it and present it to your Colleague?");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "WantedToTalkToMe")
            {
                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        TextAnimatorPlayer.ShowText(
                            "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_WantedToTalkToMe")
            {
                if (this.gameObject.CompareTag($"Boss"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_InternKnowsAboutFile")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you please get it from the Boss?");
                            ObjectiveHandler.Progress();
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "InternKnowsAboutFile")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        TextAnimatorPlayer.ShowText(
                            "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                        ObjectiveHandler.Progress();
                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_InternKnowsAboutFile")
            {
                if (this.gameObject.CompareTag($"Intern"))
                {
                    if (ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        if (Sensitive)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that.");
                        }
                        else
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            ObjectiveHandler.Progress();
                        }

                        ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Weather")
            {
                TextAnimatorPlayer.ShowText(
                    "The weather is great");
                ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Game")
            {
                TextAnimatorPlayer.ShowText(
                    "The weather is great");
                ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "HowIsItGoing")
            {
                TextAnimatorPlayer.ShowText(
                    "The weather is great");
                ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Travel")
            {
                TextAnimatorPlayer.ShowText(
                    "The weather is great");
                ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Name")
            {
                TextAnimatorPlayer.ShowText(
                    "The weather is great");
                ToggleProcessing();
                return true;
            }
        }
        else
        {
            TextAnimatorPlayer.ShowText("I'm sorry but I have no idea what you are talking about.");
            ToggleProcessing();
            return true;
        }

        return false;
    }

    private bool OfficeLevel2(Inference inference)
    {
        _delta = 10f;
        SubtitlePanel.SetActive(true);

        if (inference.IsUnderstood)
        {
            // if (MicLoudness > 6)
            // {
            //     TextAnimatorPlayer.ShowText("Don't scream at me!");
            //     return;
            // }

            switch (inference.Intent)
            {
                case "Friendly_SecretPrice":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            TextAnimatorPlayer.ShowText("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            TextAnimatorPlayer.ShowText(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "SecretPrice":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            TextAnimatorPlayer.ShowText("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            TextAnimatorPlayer.ShowText(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_SecretPrice":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            TextAnimatorPlayer.ShowText("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            TextAnimatorPlayer.ShowText(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_NeedHelp":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            TextAnimatorPlayer.ShowText("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            TextAnimatorPlayer.ShowText("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "NeedHelp":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            TextAnimatorPlayer.ShowText("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            TextAnimatorPlayer.ShowText("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_NeedHelp":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            TextAnimatorPlayer.ShowText("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            TextAnimatorPlayer.ShowText("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_Yes":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            TextAnimatorPlayer.ShowText("Yes, can you help me solve the letter?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Yes":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            TextAnimatorPlayer.ShowText("Yes, can you help me solve the letter?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_Yes":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            TextAnimatorPlayer.ShowText("Yes, can you help me solve the letter?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_5276":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            TextAnimatorPlayer.ShowText("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "5276":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            TextAnimatorPlayer.ShowText("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_5276":
                {
                    if (this.gameObject.CompareTag($"Colleague"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            TextAnimatorPlayer.ShowText("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_WantedToTalkToMe":
                {
                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            TextAnimatorPlayer.ShowText("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            TextAnimatorPlayer.ShowText(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "WantedToTalkToMe":
                {
                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            TextAnimatorPlayer.ShowText("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            TextAnimatorPlayer.ShowText(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_WantedToTalkToMe":
                {
                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            TextAnimatorPlayer.ShowText("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            TextAnimatorPlayer.ShowText(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    if (this.gameObject.CompareTag($"Boss"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            TextAnimatorPlayer.ShowText(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_Escape":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            TextAnimatorPlayer.ShowText("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Escape":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            TextAnimatorPlayer.ShowText("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_Escape":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            TextAnimatorPlayer.ShowText("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_Rhino":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            TextAnimatorPlayer.ShowText(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Rhino":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            TextAnimatorPlayer.ShowText(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_Rhino":
                {
                    if (this.gameObject.CompareTag($"Intern"))
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            TextAnimatorPlayer.ShowText(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Weather":
                    TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    ToggleProcessing();
                    return true;
                case "Game":
                    TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    ToggleProcessing();
                    return true;
                case "HowIsItGoing":
                    TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    ToggleProcessing();
                    return true;
                case "Travel":
                    TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    ToggleProcessing();
                    return true;
                case "Name":
                    TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    ToggleProcessing();
                    return true;
            }
        }
        else
        {
            TextAnimatorPlayer.ShowText("Didn't understand the command.\n");
            ToggleProcessing();
            return true;
        }

        return false;
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

    private bool IsInTalkingRange()
    {
        float playerDistanceSqr =
            (Player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= PlayerListenRange * PlayerListenRange;
    }

    private void ToggleProcessing()
    {
        isProcessing = !isProcessing;

        if (!isProcessing)
        {
            SwitchState(new ColleagueWorkingState(this));
        }
    }

    public void Listening()
    {
        isProcessing = false;
        StartProcessing();
    }

    private void StartProcessing()
    {
        if (isProcessing)
            return;

        ToggleProcessing();
        _rhinoManager.Process();
    }

    public void Subtitles()
    {
        switch (_delta)
        {
            case > 0:
                _delta -= Time.deltaTime;
                break;
            case <= 0:
                TextAnimatorPlayer.ShowText("");
                SubtitlePanel.SetActive(false);
                break;
        }
    }
}