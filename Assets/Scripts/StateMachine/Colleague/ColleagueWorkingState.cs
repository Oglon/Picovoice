using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueWorkingState : ColleagueBaseState
{
    
    Vector3 computer = new Vector3(0.7f, 1.2f, -0.16f);
    
    public ColleagueWorkingState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Target.transform.position = computer;
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