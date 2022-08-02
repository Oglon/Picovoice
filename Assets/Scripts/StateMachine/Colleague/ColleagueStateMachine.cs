using System;
using System.Collections;
using System.Collections.Generic;
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

    public Camera playerHead { get; private set; }


    public bool _isProcessing;
    public RhinoManager _rhinoManager;

    private const string
        ACCESS_KEY =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

    private void Start()
    {
        _rhinoManager = RhinoManager.Create(ACCESS_KEY, GetContextPath(), inferenceCallback);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHead = Camera.main;

        SwitchState(new ColleagueWorkingState(this));
    }

    void inferenceCallback(Inference inference)
    {
        if (!IsInTalkingRange())
            return;

        Debug.Log("Index: " + ObjectiveHandler.getCurrentIndex());

        Debug.Log(gameObject.tag);

        if (SceneManager.GetActiveScene().name == "Office Level 1")
        {
            if (inference.IsUnderstood)
            {
                if (inference.Intent == "Someone_in_my_office")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            Debug.Log("No but maybe the Boss saw something");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 2)
                        {
                            Debug.Log("Your Colleague may have seen something.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            Debug.Log(
                                "Maybe but I need your help first. There is a Cesar Cipher in your office. Can you tell me the solution?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Seen_my_file")
                {
                    Debug.Log("Seen my file");
                }
                else if (inference.Intent == "1111")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 13)
                        {
                            Debug.Log(
                                "The file is in the cleaning cabinet.");
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
                            Debug.Log(
                                "Amazing. The Intern may have an idea on how to get your file.");
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
                            Debug.Log(
                                "That was just a warmup. I've hidden a puzzle in this office and the Intern could need your help. Will you help him?");
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
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            Debug.Log(
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
                            Debug.Log(
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
                            Debug.Log(
                                "The code is 1111.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Need_help")
                {
                    if (this.gameObject.tag == "Intern")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            Debug.Log(
                                "Absolutely. I was told that it’s a color Code in the order Green, Red, Yellow and White but that's it.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "GummyBear_Solution")
                {
                    if (this.gameObject.tag == "Colleague")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            Debug.Log(
                                "Great. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
                else if (inference.Intent == "Wanted_to_talk_to_me")
                {
                    Debug.Log(gameObject.tag + " and " + inference.Intent);
                    if (this.gameObject.tag == "Boss")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            Debug.Log(
                                "We got a new cipher that contains a code. Can you solve it? Please present the solution to your Colleague");
                            ObjectiveHandler.Progress();
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
                            Debug.Log(
                                "Yes, I can unlock the cameras and see who took your file. I just need the remote access code. Can you get it from the Boss?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Didn't understand the command.\n");
                ToggleProcessing();
                return;
            }
        }
        else
        {
            if (inference.IsUnderstood)
            {
                if (inference.Intent == "Someone_in_my_office")
                {
                    if (this.gameObject.tag == "WhatsUp")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 1)
                        {
                            Debug.Log("There's a secret price but the boss knows more");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "SecretPrice")
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

                    if (this.gameObject.tag == "Need_help")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 3)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }

                        if (ObjectiveHandler.getCurrentIndex() == 7)
                        {
                            Debug.Log("Yes, can you help me with the morse code?");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Yes")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 4)
                        {
                            Debug.Log("Yes can you help me solve the letter.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "5276")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 5)
                        {
                            Debug.Log("Thank you so much, the boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Wanted_to_talk_to_me")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 6)
                        {
                            Debug.Log("Yes, the Intern needs your help");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }

                        if (ObjectiveHandler.getCurrentIndex() == 9)
                        {
                            Debug.Log(
                                "Yes, can you solve this code? Please present the solution to the intern, he told me it was unsolvable.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }

                        if (ObjectiveHandler.getCurrentIndex() == 11)
                        {
                            Debug.Log(
                                "I wanted to congratulate you on the grand price. You helped everyone in the office.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Escape")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 8)
                        {
                            Debug.Log("Thank you very much. The Boss wanted to talk to you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }

                    if (this.gameObject.tag == "Rhino")
                    {
                        if (ObjectiveHandler.getCurrentIndex() == 10)
                        {
                            Debug.Log("TI thought the code was just gibberish. Thanks. The Boss was looking for you.");
                            ObjectiveHandler.Progress();
                            ToggleProcessing();
                            return;
                        }
                    }
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
    }

    public void StartProcessing()
    {
        Debug.Log("Processing");
        if (_isProcessing)
            return;

        ToggleProcessing();
        _rhinoManager.Process();
    }
}