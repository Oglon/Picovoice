using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ColleagueTalkingState : ColleagueBaseState
{
    private float _responseTime;
    private Inference inference;
    public string Answer = "";

    Vector3 computer = new Vector3(0.7f, 1.2f, -0.16f);

    public ColleagueTalkingState(ColleagueStateMachine stateMachine, Inference Inference) : base(stateMachine)
    {
        inference = Inference;
        inferenceTalking();
    }

    public ColleagueTalkingState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
        Debug.Log("Called");
        ColleagueStateMachine.delta = 10f;
        stateMachine.SubtitlePanel.SetActive(true);
        stateMachine.NameAnimatorPlayer.ShowText(stateMachine.gameObject.tag);
        stateMachine.ToggleProcessing();
        StringPass(VolumeResponse());
    }

    public override void Enter()
    {
        stateMachine.Sprite.sprite = stateMachine.Speech;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();

        _responseTime = ColleagueStateMachine.delta;

        stateMachine.Target.transform.position = stateMachine.PlayerHead.transform.position;

        if (_responseTime <= 0)
        {
            Answer = "";
            if (IsInTalkingRange())
            {
                stateMachine.SwitchState(new ColleagueListeningState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
            }
        }

        if (SceneManager.GetActiveScene().name.Contains("Office Level 1") &&
            stateMachine.ObjectiveHandler.getCurrentIndex() == 14 && ColleagueStateMachine.delta <= 0)
        {
            ColleagueStateMachine.endCounter = 6f;
            EndLevel();
        }

        if (SceneManager.GetActiveScene().name.Contains("Office Level 2") &&
            stateMachine.ObjectiveHandler.getCurrentIndex() == 12 && ColleagueStateMachine.delta <= 0)
        {
            ColleagueStateMachine.endCounter = 6f;
            EndLevel();
        }
    }

    public override void Exit()
    {
    }

    public void inferenceTalking()
    {
        if (!IsInTalkingRange())
            return;
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Office Level 1"))
        {
            if (OfficeLevel1(inference)) return;
        }
        else
        {
            if (OfficeLevel2(inference)) return;
        }

        stateMachine.isProcessing = false;
    }

    private bool OfficeLevel1(Inference inference)
    {
        ColleagueStateMachine.delta = 10f;
        stateMachine.SubtitlePanel.SetActive(true);
        stateMachine.NameAnimatorPlayer.ShowText(stateMachine.gameObject.tag);

        if (inference.IsUnderstood)
        {
            if (inference.Intent == "Friendly_1111")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Thank you, the file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass(
                                "The file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        SceneManager.LoadScene("LevelSelection");

                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "I think you should tell the Intern about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                else if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "I just told you the code. You should tell it to the Intern.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "1111")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        StringPass(
                            "The file is in the cleaning cabinet.");
                        EndLevel();
                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "I think you should tell the Intern about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                else if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "I just told you the code. You should tell it to the Intern.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Unfriendly_1111")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "The file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        SceneManager.LoadScene("LevelSelection");
                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "I think you should tell the Intern about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                else if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "I just told you the code. You should tell it to the Intern.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }

            else if (inference.Intent == "Friendly_5276")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "Is this the code the Colleague asked you about?");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                else if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Amazing. The Intern may have an idea on how to get your file.");
                        }
                        else
                        {
                            StringPass(
                                "The Intern may have an idea on how to get your file.");
                        }

                        DespawnGameObject("PigpenCode");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    StringPass(
                        "You already told me about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                else if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "Is this the code the Colleague asked you about?.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "5276")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "Maybe talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        StringPass(
                            "Amazing. The Intern may have an idea on how to get your file.");
                        DespawnGameObject("PigpenCode");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "You should talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Unfriendly_5276")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "The Intern may have an idea on how to get your file.");
                            DespawnGameObject("PigpenCode");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                        }


                        return true;
                    }
                }
            }

            else if (inference.Intent == "Friendly_SomeoneInMyOffice")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        StringPass(
                            stateMachine.Sensitive
                                ? "Good morning. I didn't see anyone. Maybe the Boss saw someone, he is in the Director Office."
                                : "I didnt. But maybe the Boss saw something, he is in the Director Office.");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        StringPass(stateMachine.Sensitive
                            ? "Sadly, I did not. Maybe your Colleague saw someone. She is in the Management Office."
                            : "Your Colleague may have seen something. She is in the Management Office.");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        StringPass(
                            stateMachine.Sensitive
                                ? "I could help you if you do a few thins for me beforehand. There is a Cesar Cipher on my table. Can you tell me the solution?"
                                : "I didnt but I have an idea. You need to help me first though. There is a Cesar Cipher on my table. Can you tell me the solution?");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "SomeoneInMyOffice")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        StringPass("I didnt. But maybe the Boss saw something, he is in the Director Office.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        StringPass("Your Colleague may have seen something. She is in the Management Office.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        StringPass(
                            "I didnt but I have an idea. You need to help me first though. There is a Cesar Cipher on my table. Can you tell me the solution?");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        SpawnGameObject(Code.Cesar, ColleagueType.Colleague);
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_SomeoneInMyOffice")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass("I didnt. But maybe the Boss saw something, he is in the Director Office.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass("Your Colleague may have seen something. She is in the Management Office.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "I didnt but I have an idea. You need to help me first though. There is a Cesar Cipher on my table. Can you tell me the solution?");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }

            else if (inference.Intent == "Friendly_Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        StringPass(
                            stateMachine.Sensitive
                                ? "Thank you su much, could you maybe help the Intern as well?"
                                : "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");

                        DespawnGameObject("CesarCode");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        StringPass(
                            "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                        DespawnGameObject("CesarCode");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Unfriendly_Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                            DespawnGameObject("CesarCode");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }

            else if (inference.Intent == "Friendly_Yes")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass("Amazing, thank you so much!.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass("Great, he will tell you the rest.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass("Amazing.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass("Great.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Of course, the code is 1111.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass("The code is 1111.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Yes")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                    {
                        StringPass(
                            "Great, he will tell you the rest.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        StringPass(
                            "Great.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        StringPass(
                            "The code is 1111.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_Yes")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "Great, he will tell you the rest.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            StringPass(
                                "Great.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            StringPass(
                                "The Code is 1111.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }

            else if (inference.Intent == "Friendly_NeedHelp")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass(
                                "Yes. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        SpawnGameObject(Code.GummyBear, ColleagueType.Intern);
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "NeedHelp")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        StringPass(
                            "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                        stateMachine.ObjectiveHandler.Progress();
                        SpawnGameObject(Code.GummyBear, ColleagueType.Intern);
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Unfriendly_NeedHelp")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.GummyBear, ColleagueType.Intern);
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }

            else if (inference.Intent == "Friendly_GummyBearSolution")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Great. The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        DespawnGameObject("GummyBearCode");
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Wow, Thank you. Can you tell the solution to your Colleague as well?");
                        }
                        else
                        {
                            StringPass(
                                "Thanks. Can you tell the solution to your Colleague as well?");
                        }
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }
            else if (inference.Intent == "GummyBearSolution")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        StringPass(
                            "The Boss wanted to talk to you.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        DespawnGameObject("GummyBearCode");
                        return true;
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        {
                            StringPass(
                                "Thanks. Can you tell the solution to your Colleague as well?");
                        }
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }
            else if (inference.Intent == "Unfriendly_GummyBearSolution")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                        else
                        {
                            StringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            DespawnGameObject("GummyBearCode");
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                }
                else if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                        else
                        {
                            StringPass(
                                "Thanks. Can you tell the solution to your Colleague as well?");
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }

            else if (inference.Intent == "Friendly_WantedToTalkToMe")
            {
                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        if (stateMachine.Sensitive)
                        {
                            //TODO: Test
                            StringPass(
                                "We got a new cipher that contains a code. Can you please solve it and present it to your Colleague?");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }
            else if (inference.Intent == "WantedToTalkToMe")
            {
                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        StringPass(
                            "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague.");
                        stateMachine.ObjectiveHandler.Progress();
                        SpawnGameObject(Code.Pigpen, ColleagueType.Boss);
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }
            else if (inference.Intent == "Unfriendly_WantedToTalkToMe")
            {
                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }

            else if (inference.Intent == "Friendly_InternKnowsAboutFile")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you please get it from the Boss?");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            StringPass(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }
            else if (inference.Intent == "InternKnowsAboutFile")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        StringPass(
                            "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }
            else if (inference.Intent == "Unfriendly_InternKnowsAboutFile")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        if (stateMachine.Sensitive)
                        {
                            StringPass(UnfriendlyResponse());
                        }
                        else
                        {
                            StringPass(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
                else
                {
                    NotUnderstood();
                }
            }

            else if (inference.Intent == "Curse")
            {
                StringPass(
                    CurseResponse());
                stateMachine.ToggleProcessing();
                return true;
            }

            else if (inference.Intent == "Weather")
            {
                StringPass(
                    "Oh the weather is amazing!");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Game")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "I don't have time for that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "I don't watch sports.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "Not this time.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                StringPass(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "HowIsItGoing")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "Barely holding on.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "As usual.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "Great, as always.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Travel")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "I don't have time for that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "France if I have the time.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "The only important thing for me is luxury.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Name")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "Mike, I don't think I introduced myself before.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "Anna. You know that. We are Colleagues for years.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "We are not on first name basis");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "WorkingOn")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "Just some cipher tasks.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "Developing new cipher cracking algorithms.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "My financial baseline.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Hobbies")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "Watching Movies and playing Games.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "Knitting as you should know.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "Buying houses and cars.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "FavoriteMovie")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "There are too many good ones to chose.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "Good Question. Maybe The Thing");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "Wolf of Wall Street.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "News")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "No time for that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "I'm always up to date.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "I make the news.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Weekend")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "I will be in the office.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "Maybe going to the park.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "Driving up to my vacation home.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "HowDoYouLikeItHere")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    StringPass(
                        "I like it but it's stressful.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    StringPass(
                        "Better than my last job.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    StringPass(
                        "You could work more but otherwise it's okay.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
        }
        else
        {
            return NotUnderstood();
        }

        if (string.IsNullOrEmpty(Answer))
        {
            NotUnderstood();
        }

        return false;
    }

    private bool OfficeLevel2(Inference inference)
    {
        ColleagueStateMachine.delta = 10f;
        stateMachine.SubtitlePanel.SetActive(true);
        stateMachine.NameAnimatorPlayer.ShowText(stateMachine.gameObject.tag);

        if (inference.IsUnderstood)
        {
            switch (inference.Intent)
            {
                case "Friendly_SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            StringPass(
                                "There's a secret price but the boss knows more");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        StringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            StringPass(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        StringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            StringPass(
                                "There's a secret price but the boss knows more");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        StringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            StringPass(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        StringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "There's a secret price but the boss knows more");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }

                        StringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }

                        StringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Friendly_NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            StringPass("Yes can you help me solve the letter.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            StringPass("Yes, can you help me with the morse code?");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Morse, ColleagueType.Intern);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            StringPass("Yes can you help me solve the letter.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            StringPass("Yes, can you help me with the morse code?");
                            SpawnGameObject(Code.Morse, ColleagueType.Intern);
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass("Yes can you help me solve the letter.");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass("Yes, can you help me with the morse code?");
                                SpawnGameObject(Code.Morse, ColleagueType.Intern);
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Friendly_Yes":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            StringPass("Thank you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SpawnGameObject(Code.Tuple, ColleagueType.Colleague);
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Yes":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            StringPass("Thank you.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Tuple, ColleagueType.Colleague);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_Yes":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass("Thank you.");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Friendly_5276":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            StringPass(
                                "Thank you so much, the Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            DespawnGameObject("TupleCode");
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "5276":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            StringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            DespawnGameObject("TupleCode");
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_5276":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "The Boss wanted to talk to you.");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                DespawnGameObject("TupleCode");
                                return true;
                            }
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Friendly_WantedToTalkToMe":
                {
                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            StringPass("Yes, the Intern needs your help");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            StringPass(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Element, ColleagueType.Boss);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            StringPass(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SceneManager.LoadScene("LevelSelection");
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "WantedToTalkToMe":
                {
                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            StringPass("Yes, the Intern needs your help");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            StringPass(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Element, ColleagueType.Boss);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            StringPass(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SceneManager.LoadScene("LevelSelection");
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_WantedToTalkToMe":
                {
                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass("Yes, the Intern needs your help");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                                stateMachine.ObjectiveHandler.Progress();
                                SpawnGameObject(Code.Element, ColleagueType.Boss);
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                SceneManager.LoadScene("LevelSelection");
                                return true;
                            }
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Friendly_Escape":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            StringPass(
                                "Thank you very much. The Boss wanted to talk to you.");
                            DespawnGameObject("MorseCode");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Escape":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            StringPass(
                                "Thank you very much. The Boss wanted to talk to you.");
                            DespawnGameObject("MorseCode");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_Escape":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "Thank you very much. The Boss wanted to talk to you.");
                                DespawnGameObject("MorseCode");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Friendly_Rhino":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            StringPass(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            DespawnGameObject("ElementCode");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Rhino":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            StringPass(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            DespawnGameObject("ElementCode");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }
                case "Unfriendly_Rhino":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            if (stateMachine.Sensitive)
                            {
                                StringPass(UnfriendlyResponse());
                            }
                            else
                            {
                                StringPass(
                                    "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                                DespawnGameObject("ElementCode");
                                stateMachine.ObjectiveHandler.Progress();
                                stateMachine.ToggleProcessing();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        NotUnderstood();
                    }

                    break;
                }

                case "Curse":
                {
                    StringPass(
                        CurseResponse());
                    stateMachine.ToggleProcessing();
                    return true;
                }

                case "Weather":
                {
                    StringPass(
                        "Oh the weather is amazing!");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                case "Game":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "I don't have time for that.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "I don't watch sports.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "Not this time.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "HowIsItGoing":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "Barely holding on.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "As usual.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "Great, as always.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "Travel":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "I don't have time for that.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "I don't watch sports.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "Not this time.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "Name":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "Mike, I don't think I introduced myself before.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "Anna. You know that. We are Colleagues for years");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "We are not on first name basis");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "WorkingOn":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "Just some cipher tasks.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "Developing new cipher cracking algorithms.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "My financial baseline.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "Hobbies":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "Watching Movies and playing Games.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "Knitting as you should know.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "Buying houses and cars.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "FavoriteMovie":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "There are too many good ones to chose.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "Good Question. Maybe The Thing");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "Wolf of Wall Street.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "News":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "No time for that.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "I'm always up to date.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "I make the news.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "Weekend":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "I will be in the office.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "Maybe going to the park.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "Driving up to my vacation home.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "HowDoYouLikeItHere":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        StringPass(
                            "I like it but it's stressful.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        StringPass(
                            "Better than my last job.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        StringPass(
                            "You could work more but otherwise it's okay.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
            }

            NotUnderstood();
        }

        if (string.IsNullOrEmpty(Answer))
        {
            NotUnderstood();
        }

        return false;
    }

    private bool NotUnderstood()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 11);
        string responseString = "";

        switch (responseInt)
        {
            case 1:
                responseString = "I'm sorry but I have no idea what you are talking about.";
                break;

            case 2:
                responseString = "Could you repeat that.";
                break;

            case 3:
                responseString = "What are you talking about.";
                break;

            case 4:
                responseString = "I think you should rephrase that.";
                break;

            case 5:
                responseString = "Could you maybe ask something else.";
                break;

            case 6:
                responseString = "I have no idea what you are talking about.";
                break;

            case 7:
                responseString = "I didn't understand you.";
                break;

            case 8:
                responseString = "Could you talk more clearly.";
                break;

            case 9:
                responseString = "Could you repeat that.";
                break;

            case 10:
                responseString = "Come again.";
                break;
        }

        StringPass(responseString);
        stateMachine.ToggleProcessing();
        return true;
    }

    private void SpawnGameObject(Code code, ColleagueType colleagueType)
    {
        GameObject place;
        if (colleagueType == ColleagueType.Intern)
        {
            place = GameObject.Find("Spawner_Intern");
        }
        else if (colleagueType == ColleagueType.Colleague)
        {
            place = GameObject.Find("Spawner_Colleague");
        }
        else
        {
            place = GameObject.Find("Spawner_Boss");
        }

        string stringCode = code + "Code";

        GameObject gameObject = GameObject.Find(stringCode);
        gameObject.transform.position = place.transform.position;
    }

    private void DespawnGameObject(string objectName)
    {
        PickUpController pickUpController = GameObject.Find(objectName).GetComponent<PickUpController>();
        pickUpController.Delete();
    }

    private void StringPass(string response)
    {
        Answer = response;
        if (SceneManager.GetActiveScene().name.Contains("Sensitive"))
        {
            stateMachine.ObjectiveHandler.Slider.SetActive(false);
        }

        stateMachine.DialogueAnimatorPlayer.ShowText(response);
        HandleAudio(response);
    }

    private void PlayAudio(AudioClip audioClip)
    {
        stateMachine.AudioSource.clip = audioClip;
        stateMachine.AudioSource.Play();
    }

    private AudioClip GetAudioClip(string title)
    {
        var str = title;
        var charsToRemove = new string[] { "!", "?" };
        foreach (var c in charsToRemove)
        {
            str = str.Replace(c, string.Empty);
        }

        if (str.EndsWith("."))
        {
            str = str.Remove(str.Length - 1);
        }

        Debug.Log(str);
        Debug.Log(stateMachine.ObjectiveHandler.getCurrentIndex());

        string clipPath = "Audio/" + stateMachine.ColleagueType.ToString() + "/";
        clipPath = clipPath + str;
        
        Debug.Log(clipPath);

        AudioClip clip = Resources.Load<AudioClip>(clipPath);
        return clip;
    }

    private void HandleAudio(string title)
    {
        AudioClip audioClip = GetAudioClip(title);
        float length;
        length = audioClip.length * 3;
        ColleagueStateMachine.delta = length;
        PlayAudio(audioClip);
    }

    private string CurseResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 5);
        string responseString = "";

        switch (responseInt)
        {
            case 1:
                responseString = "Don't use these words around me!";
                break;

            case 2:
                responseString = "Don't talk to me like that!";
                break;

            case 3:
                responseString = "I don't appreciate you using these words.";
                break;

            case 4:
                responseString = "No curse words around here!";
                break;
        }

        return responseString;
    }

    private string UnfriendlyResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 4);
        string responseString = "";

        switch (responseInt)
        {
            case 1:
                responseString = "I don't like your tone!";
                break;

            case 2:
                responseString = "I think you could say that in a nicer way!";
                break;

            case 3:
                responseString = "Tone matters around here.";
                break;
        }

        return responseString;
    }

    private string VolumeResponse()
    {
        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 4);
        string responseString = "";

        switch (responseInt)
        {
            case 1:
                responseString = "Stop shouting.";
                break;

            case 2:
                responseString = "Tone down your voice.";
                break;

            case 3:
                responseString = "Don't scream at me.";
                break;
        }

        return responseString;
    }

    private void EndLevel()
    {
        if (ColleagueStateMachine.endCounter <= 0)
        {
            SceneManager.LoadScene("LevelSelection");
        }

        ColleagueStateMachine.endCounter -= Time.deltaTime;

        stateMachine.ObjectiveHandler.Icon.gameObject.SetActive(true);

        stateMachine.ObjectiveHandler.StartPanel.SetActive(true);
        stateMachine.ObjectiveHandler.StartTMP.gameObject.SetActive(true);
        if (SceneManager.GetActiveScene().name.Contains("Office Level 1"))
        {
            stateMachine.ObjectiveHandler.startText =
                "Congratulations! You helped your Colleagues with some puzzles and found the location of your file.";
        }
        else
        {
            stateMachine.ObjectiveHandler.startText =
                "Congratulations! You helped your Colleagues and won the secret prize.";
        }

        stateMachine.ObjectiveHandler.StartTMP.text = stateMachine.ObjectiveHandler.startText;
    }
}