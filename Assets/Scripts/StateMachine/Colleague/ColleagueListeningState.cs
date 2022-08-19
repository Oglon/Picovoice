using System.Collections;
using System.Collections.Generic;
using Pv.Unity;
using UnityEngine;

public class ColleagueListeningState : ColleagueBaseState
{
    public ColleagueListeningState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("In Talking Range");
        stateMachine.StartProcessing();
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();
        stateMachine.Target.transform.position = stateMachine.playerHead.transform.position;

        if (!IsInTalkingRange())
        {
            stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}