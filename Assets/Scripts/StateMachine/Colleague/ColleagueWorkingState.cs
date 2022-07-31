using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueWorkingState : ColleagueBaseState
{
    public ColleagueWorkingState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        if (IsInTalkingRange())
        {
            stateMachine.SwitchState(new ColleagueListeningState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}