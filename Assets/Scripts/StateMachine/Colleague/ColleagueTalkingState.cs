using System;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;


public class ColleagueTalkingState : ColleagueBaseState
{
    private float _responseTime = 5f;
    private Inference inference;

    Vector3 computer = new Vector3(0.7f, 1.2f, -0.16f);

    public ColleagueTalkingState(ColleagueStateMachine stateMachine, Inference Inference) : base(stateMachine)
    {
        inference = Inference;
        inferenceTalking();
    }

    public override void Enter()
    {
        stateMachine.Sprite.sprite = stateMachine.Speech;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();

        _responseTime -= deltaTime;

        stateMachine.Target.transform.position = stateMachine.PlayerHead.transform.position;

        if (_responseTime <= 0)
        {
            stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
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

        if (stateMachine.Loudness.returnValue >= 0.5f)
        {
            if (!inference.Intent.Contains("Unfriendly_"))
            {
                inference.Intent = "Unfriendly_" + inference.Intent;
            }
        }

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
                            stringPass(
                                "Thank you, the file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass(
                                "The file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        SceneManager.LoadScene("LevelSelection");
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "I think you should tell the Intern about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
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
                        stringPass(
                            "The file is in the cleaning cabinet.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        SceneManager.LoadScene("LevelSelection");
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "I think you should tell the Intern about the code.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
                            "I just told you the code. You should tell it to the Intern.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stringPass(
                                "I don't appreciate your rudeness but the file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass(
                                "The file is in the cleaning cabinet.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        SceneManager.LoadScene("LevelSelection");
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "I think you should tell the Intern about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "I just told you the code. You should tell it to the Intern.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }

            else if (inference.Intent == "Friendly_5276")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "Is this the code the Colleague asked you about?.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        if (stateMachine.Sensitive)
                        {
                            stringPass(
                                "Amazing. The Intern may have an idea on how to get your file.");
                        }
                        else
                        {
                            stringPass(
                                "The Intern may have an idea on how to get your file.");
                        }

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    stringPass(
                        "You already told me about the code.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "Is this the code the Colleague asked you about?.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "5276")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "Maybe talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        stringPass(
                            "Amazing. The Intern may have an idea on how to get your file.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
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
                            stringPass(
                                "Be nicer next time. The Intern may have an idea on how to get your file.");
                        }
                        else
                        {
                            stringPass(
                                "The Intern may have an idea on how to get your file.");
                        }

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
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
                        stringPass(
                            stateMachine.Sensitive
                                ? "Good morning. I didn't see anyone. Maybe the Boss saw someone?"
                                : "No but maybe the Boss saw something");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        stringPass(stateMachine.Sensitive
                            ? "Sadly, I did not. Maybe your Colleague saw someone?"
                            : "Your Colleague may have seen something.");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        stringPass(
                            stateMachine.Sensitive
                                ? "Well I might have seen something but I need your help first. There is a Cesar Cipher on my table. Can you tell me the solution?"
                                : "Maybe but I need your help first. There is a Cesar Cipher on my table. Can you tell me the solution?");

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
                        stringPass("No but maybe the Boss saw something");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        stringPass("Your Colleague may have seen something.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        stringPass(
                            "Maybe but I need your help first. There is a Cesar Cipher on my table. Can you tell me the solution?");
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
                            stringPass("Don't talk to me like that");
                        }
                        else
                        {
                            stringPass("No but maybe the Boss saw something");
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
                            stringPass("Don't talk to me like that");
                        }
                        else
                        {
                            stringPass("Your Colleague may have seen something.");
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
                            stringPass("Don't talk to me like that.");
                        }
                        else
                        {
                            stringPass(
                                "Maybe but I need your help first. There is a Cesar Cipher on my table. Can you tell me the solution?");
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
                    stringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        stringPass(
                            stateMachine.Sensitive
                                ? "Thank you su much, could you maybe help the Intern as well?"
                                : "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");

                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        stringPass(
                            "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Unfriendly_Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass("Talk to your Colleague about that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        if (stateMachine.Sensitive)
                        {
                            stringPass("Don't talk to me like that.");
                        }
                        else
                        {
                            stringPass(
                                "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass("Talk to your Colleague about that.");
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
                            stringPass("Amazing, thank you so much!.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass("Great, he will tell you the rest.");
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
                            stringPass("Amazing.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass("Great.");
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
                            stringPass(
                                "Of course, the code is 1111.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass("The code is 1111.");
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
                        stringPass(
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
                        stringPass(
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
                        stringPass(
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
                            stringPass(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            stringPass(
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
                            stringPass(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            stringPass(
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
                            stringPass(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            stringPass(
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
                            stringPass(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass(
                                "Yes. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        SpawnGameObject(Code.GummyBear, ColleagueType.Intern);
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    stringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
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
                        stringPass(
                            "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                        stateMachine.ObjectiveHandler.Progress();
                        SpawnGameObject(Code.GummyBear, ColleagueType.Intern);
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
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
                            stringPass(
                                "Please don't talk to me like that.");
                        }
                        else
                        {
                            stringPass(
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
                    stringPass(
                        "No, not at the moment.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
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
                            stringPass(
                                "Great. The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "GummyBearSolution")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                    {
                        stringPass(
                            "The Boss wanted to talk to you.");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        DespawnGameObject("GummyBearCode");
                        return true;
                    }
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
                            stringPass(
                                "Don't talk to me like that.");
                        }
                        else
                        {
                            stringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            DespawnGameObject("GummyBearCode");
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stringPass(
                                "We got a new cipher that contains a code. Can you please solve it and present it to your Colleague?");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "WantedToTalkToMe")
            {
                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                    {
                        stringPass(
                            "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                        stateMachine.ObjectiveHandler.Progress();
                        SpawnGameObject(Code.Pigpen, ColleagueType.Boss);
                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stringPass(
                                "Don't talk to me like that!");
                        }
                        else
                        {
                            stringPass(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stringPass(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you please get it from the Boss?");
                            stateMachine.ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stringPass(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "InternKnowsAboutFile")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                    {
                        stringPass(
                            "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                        stateMachine.ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stringPass(
                                "Don't talk to me like that.");
                        }
                        else
                        {
                            stringPass(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            stateMachine.ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }

            else if (inference.Intent == "Weather")
            {
                stringPass(
                    "Oh the weather is amazing!");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Game")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "I don't have time for that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "I don't watch sports.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "Not this time.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                stringPass(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "HowIsItGoing")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "Barely holding on.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "As usual.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "Great, as always.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Travel")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "I don't have time for that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "France if I have the time.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "The only important thing for me is luxury.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Name")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "Mike, I don't think I introduced myself before.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "Anna. You know that. We are Colleagues for years.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "We are not on first name basis");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "WorkingOn")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "Just some cipher tasks.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "Developing new cipher cracking algorithms.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "My financial baseline.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Hobbys")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "Watching Movies and playing Games.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "Knitting as you should know.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "Buying houses and cars.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "FavoriteMovie")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "There are too many good ones to chose.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "Good Question. Maybe The Thing");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "Wolf of Wall Street.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "News")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "No time for that.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "I'm always up to date.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "I make the news.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "Weekend")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "I will be in the office.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "Maybe going to the park.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "Driving up to my vacation home.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
            else if (inference.Intent == "HowDoYouLikeItHere")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    stringPass(
                        "I like it but it's stressful.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    stringPass(
                        "Better than my last job.");
                    stateMachine.ToggleProcessing();
                    return true;
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    stringPass(
                        "You could work more but otherwise it's okay.");
                    stateMachine.ToggleProcessing();
                    return true;
                }
            }
        }
        else
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

            stringPass(responseString);
            stateMachine.ToggleProcessing();
            return true;
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
                            stringPass(
                                "There's a secret price but the boss knows more");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        stringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            stringPass(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        stringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            stringPass(
                                "There's a secret price but the boss knows more");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        stringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            stringPass(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        stringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "Unfriendly_SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            stringPass(
                                "There's a secret price but the boss knows more");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        stringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            stringPass(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }

                        stringPass(
                            "We talked about that");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }

                case "Friendly_NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            stringPass("Yes can you help me solve the letter.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            stringPass("Yes, can you help me with the morse code?");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Morse, ColleagueType.Intern);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            stringPass("Yes can you help me solve the letter.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            stringPass("Yes, can you help me with the morse code?");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            stringPass("Yes can you help me solve the letter.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            stringPass("Yes, can you help me with the morse code?");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }

                case "Friendly_Yes":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            stringPass("Yes, can you help me solve the letter?");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SpawnGameObject(Code.Tuple, ColleagueType.Colleague);
                            return true;
                        }
                    }

                    break;
                }
                case "Yes":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            stringPass("Yes, can you help me solve the letter?");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Tuple, ColleagueType.Colleague);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_Yes":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            stringPass("Yes, can you help me solve the letter?");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }

                case "Friendly_5276":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            stringPass(
                                "Thank you so much, the Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "5276":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            stringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_5276":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            stringPass(
                                "The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }

                case "Friendly_WantedToTalkToMe":
                {
                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            stringPass("Yes, the Intern needs your help");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            stringPass(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Element, ColleagueType.Intern);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            stringPass(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SceneManager.LoadScene("LevelSelection");
                            return true;
                        }
                    }

                    break;
                }
                case "WantedToTalkToMe":
                {
                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            stringPass("Yes, the Intern needs your help");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            stringPass(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Element, ColleagueType.Intern);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            stringPass(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SceneManager.LoadScene("LevelSelection");
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_WantedToTalkToMe":
                {
                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            stringPass("Yes, the Intern needs your help");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            stringPass(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            stateMachine.ObjectiveHandler.Progress();
                            SpawnGameObject(Code.Element, ColleagueType.Intern);
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            stringPass(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            SceneManager.LoadScene("LevelSelection");
                            return true;
                        }
                    }

                    break;
                }

                case "Friendly_Escape":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            stringPass(
                                "Thank you very much. The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Escape":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            stringPass(
                                "Thank you very much. The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_Escape":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            stringPass(
                                "Thank you very much. The Boss wanted to talk to you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }

                case "Friendly_Rhino":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            stringPass(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Rhino":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            stringPass(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_Rhino":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            stringPass(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            stateMachine.ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }

                case "Weather":
                {
                    stringPass(
                        "Oh the weather is amazing!");
                    stateMachine.ToggleProcessing();
                    return true;
                }
                case "Game":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        stringPass(
                            "I don't have time for that.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "I don't watch sports.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "Barely holding on.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "As usual.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "I don't have time for that.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "I don't watch sports.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "Mike, I don't think I introduced myself before.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "Anna. You know that. We are Colleagues for years");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "Just some cipher tasks.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "Developing new cipher cracking algorithms.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
                            "My financial baseline.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
                case "Hobbys":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        stringPass(
                            "Watching Movies and playing Games.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "Knitting as you should know.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "There are too many good ones to chose.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "Good Question. Maybe The Thing");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "No time for that.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "I'm always up to date.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "I will be in the office.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "Maybe going to the park.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
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
                        stringPass(
                            "I like it but it's stressful.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        stringPass(
                            "Better than my last job.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        stringPass(
                            "You could work more but otherwise it's okay.");
                        stateMachine.ToggleProcessing();
                        return true;
                    }

                    break;
                }
            }

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

            HandleAudio(responseString);
            stringPass(responseString);
            stateMachine.ToggleProcessing();
            return true;
        }

        return false;
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
        GameObject gameObject = GameObject.Find(objectName);
        GameObject.Destroy(gameObject);
    }

    private void stringPass(string response)
    {
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
        string clipPath = "Audio/" + stateMachine.ColleagueType.ToString() + "/";
        clipPath = clipPath + title;

        AudioClip clip = Resources.Load<AudioClip>(clipPath);
        return clip;
    }

    private void HandleAudio(string title)
    {
        AudioClip audioClip = GetAudioClip(title);
        float length;
        length = audioClip.length <= 10 ? 10f : audioClip.length;
        ColleagueStateMachine.delta = length;
        PlayAudio(audioClip);
    }
}