using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColleagueWorkingState : ColleagueBaseState
{
    public ColleagueWorkingState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Target.transform.position = stateMachine.MainTarget.transform.position;
        stateMachine.Sprite.sprite = stateMachine.Working;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();
        if (IsInTalkingRange())
        {
            stateMachine.SwitchState(new ColleagueListeningState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}