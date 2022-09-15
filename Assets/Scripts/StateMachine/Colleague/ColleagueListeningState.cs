using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pv.Unity;
using UnityEngine;

public class ColleagueListeningState : ColleagueBaseState
{
    public ColleagueListeningState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Listening();
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();

        if (stateMachine.gameObject.tag == "Colleague")
        {
            var lookPos = stateMachine.PlayerHead.transform.position - stateMachine.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            stateMachine.transform.rotation =
                Quaternion.Slerp(stateMachine.transform.rotation, rotation, Time.deltaTime);
        }

        stateMachine.Target.transform.position = stateMachine.PlayerHead.transform.position;

        if (!IsInTalkingRange())
        {
            stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}