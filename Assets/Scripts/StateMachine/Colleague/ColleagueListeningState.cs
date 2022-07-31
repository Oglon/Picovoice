using System.Collections;
using System.Collections.Generic;
using Pv.Unity;
using UnityEngine;

public class ColleagueListeningState : ColleagueBaseState
{
    private bool _isProcessing;
    RhinoManager _rhinoManager;

    private const string
        ACCESS_KEY =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";


    public ColleagueListeningState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override void Enter()
    {
        Debug.Log("In Talking Range");
        _rhinoManager = RhinoManager.Create(ACCESS_KEY, GetContextPath(), inferenceCallback);
        ToggleProcessing();
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInTalkingRange())
        {
            stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }

    private void ToggleProcessing()
    {
        if (!_isProcessing)
        {
            StartProcessing();
        }
    }
    
    private void StartProcessing()
    {
        _rhinoManager.Process();
        _isProcessing = true;
    }
    
    void inferenceCallback(Inference inference)
    {
        if (inference.IsUnderstood)
        {
            if (inference.Intent == "Someone_in_my_office")
            {
                Debug.Log("Someone in my office");
            }
            else if (inference.Intent == "Seen_my_file")
            {
                Debug.Log("Seen my file");
            }
        }
        else
        {
            Debug.Log("Didn't understand the command.\n");
            stateMachine.SwitchState(new ColleagueTalkingState(stateMachine));
        }

        _isProcessing = false;
    }

    private static string GetContextPath()
    {
        return "E:/UnityProjects/Picovoice/Assets/StreamingAssets/contexts/windows/colleague_windows.rhn";
    }
}