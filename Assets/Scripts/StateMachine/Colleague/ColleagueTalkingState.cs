using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (_responseTime <=0)
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
        stateMachine.NamePanel.SetActive(true);
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
            if (inference.Intent == "Friendly_SomeoneInMyOffice")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
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
                        stateMachine.TextAnimatorPlayer.ShowText(stateMachine.Sensitive
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
                        stateMachine.TextAnimatorPlayer.ShowText(
                            stateMachine.Sensitive
                                ? "Well I might have seen something but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?"
                                : "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");

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
                        stateMachine.TextAnimatorPlayer.ShowText("No but maybe the Boss saw something");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText("Your Colleague may have seen something.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
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
                        if ( stateMachine.Sensitive)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Don't talk to me like that");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("No but maybe the Boss saw something");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Don't talk to me like that");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Your Colleague may have seen something.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Don't talk to me like that.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                           stateMachine. ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_1111")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        if (stateMachine.Sensitive)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "The file is in the cleaning cabinet.");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "The file is in the cleaning cabinet.");
                           stateMachine. ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "1111")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 13)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "The file is in the cleaning cabinet.");
                       stateMachine. ObjectiveHandler.Progress();
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
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "The file is in the cleaning cabinet.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_5276")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Amazing. The Intern may have an idea on how to get your file.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "5276")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Amazing. The Intern may have an idea on how to get your file.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_5276")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Amazing. The Intern may have an idea on how to get your file.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Friendly_Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            stateMachine.Sensitive
                                ? "Thank you su much, could you maybe help the Intern as well?"
                                : "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");

                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Unfriendly_Afoot")
            {
                if (stateMachine.gameObject.CompareTag($"Colleague"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 4)
                    {
                        if (stateMachine.Sensitive)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Don't talk to me like that.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
                           stateMachine. ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stateMachine.TextAnimatorPlayer.ShowText("Amazing, thank you so much!.");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Great, he will tell you the rest.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Amazing.");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Great.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Of course, the code is 1111.");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("The code is 1111.");
                           stateMachine. ObjectiveHandler.Progress();
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
                        stateMachine.TextAnimatorPlayer.ShowText(
                            " Great, he will tell you the rest.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Great.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }

                if (stateMachine.gameObject.CompareTag($"Boss"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 12)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "The code is 1111.");
                       stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                " Great, he will tell you the rest.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Great.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "The Code is 1111.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                           stateMachine. ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "NeedHelp")
            {
                if (stateMachine.gameObject.CompareTag($"Intern"))
                {
                    if (stateMachine.ObjectiveHandler.getCurrentIndex() == 6)
                    {
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Please don't talk to me like that.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                           stateMachine. ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Great. The Boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "The Boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "The Boss wanted to talk to you.");
                       stateMachine. ObjectiveHandler.Progress();
                        stateMachine.ToggleProcessing();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "The Boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "We got a new cipher that contains a code. Can you please solve it and present it to your Colleague?");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                           stateMachine. ObjectiveHandler.Progress();
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
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                       stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that!");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you please get it from the Boss?");
                           stateMachine. ObjectiveHandler.Progress();
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                           stateMachine. ObjectiveHandler.Progress();
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
                        stateMachine.TextAnimatorPlayer.ShowText(
                            "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                       stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Don't talk to me like that.");
                        }
                        else
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                           stateMachine. ObjectiveHandler.Progress();
                        }

                        stateMachine.ToggleProcessing();
                        return true;
                    }
                }
            }
            else if (inference.Intent == "Weather")
            {
                stateMachine.TextAnimatorPlayer.ShowText(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Game")
            {
                stateMachine.TextAnimatorPlayer.ShowText(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "HowIsItGoing")
            {
                stateMachine.TextAnimatorPlayer.ShowText(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Travel")
            {
                stateMachine.TextAnimatorPlayer.ShowText(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
            else if (inference.Intent == "Name")
            {
                stateMachine.TextAnimatorPlayer.ShowText(
                    "The weather is great");
                stateMachine.ToggleProcessing();
                return true;
            }
        }
        else
        {
            stateMachine.TextAnimatorPlayer.ShowText("I'm sorry but I have no idea what you are talking about.");
            stateMachine.ToggleProcessing();
            return true;
        }

        return false;
    }

    private bool OfficeLevel2(Inference inference)
    {
        ColleagueStateMachine.delta = 10f;
        stateMachine.SubtitlePanel.SetActive(true);
        stateMachine.NamePanel.SetActive(true);
        stateMachine. NameAnimatorPlayer.ShowText(stateMachine.gameObject.tag);

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
                            stateMachine.TextAnimatorPlayer.ShowText("There's a secret price but the boss knows more");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("There's a secret price but the boss knows more");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Unfriendly_SecretPrice":
                {
                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("There's a secret price but the boss knows more");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "That's right, it’s for the most productive employee. You should maybe assist your Colleague.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Friendly_NeedHelp":
                {
                    if (stateMachine.gameObject.CompareTag($"Colleague"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Yes can you help me solve the letter.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, can you help me with the morse code?");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes can you help me solve the letter.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, can you help me with the morse code?");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes can you help me solve the letter.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Intern"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, can you help me with the morse code?");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, can you help me solve the letter?");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, can you help me solve the letter?");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, can you help me solve the letter?");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Thank you so much, the boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Thank you so much, the boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Thank you so much, the boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, the Intern needs your help");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, the Intern needs your help");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Yes, the Intern needs your help");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    if (stateMachine.gameObject.CompareTag($"Boss"))
                    {
                        if (stateMachine.ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Thank you very much. The Boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Thank you very much. The Boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText("Thank you very much. The Boss wanted to talk to you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                           stateMachine. ObjectiveHandler.Progress();
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
                            stateMachine.TextAnimatorPlayer.ShowText(
                                "I thought the code was just gibberish. Thanks. The Boss was looking for you.");
                           stateMachine. ObjectiveHandler.Progress();
                            stateMachine.ToggleProcessing();
                            return true;
                        }
                    }

                    break;
                }
                case "Weather":
                    stateMachine.TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    stateMachine.ToggleProcessing();
                    return true;
                case "Game":
                    stateMachine.TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    stateMachine.ToggleProcessing();
                    return true;
                case "HowIsItGoing":
                    stateMachine.TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    stateMachine.ToggleProcessing();
                    return true;
                case "Travel":
                    stateMachine.TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    stateMachine.ToggleProcessing();
                    return true;
                case "Name":
                    stateMachine.TextAnimatorPlayer.ShowText(
                        "The weather is great");
                    stateMachine.ToggleProcessing();
                    return true;
            }
        }
        else
        {
            stateMachine.TextAnimatorPlayer.ShowText("Didn't understand the command.\n");
            stateMachine.ToggleProcessing();
            return true;
        }

        return false;
    }
    
}