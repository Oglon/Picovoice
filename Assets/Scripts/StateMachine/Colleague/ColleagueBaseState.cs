using System;
using UnityEngine;

public abstract class ColleagueBaseState : State
{
    protected ColleagueStateMachine stateMachine;

    public ColleagueBaseState(ColleagueStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool IsInTalkingRange()
    {
        float playerDistanceSqr =
            (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.PlayerListenRange * stateMachine.PlayerListenRange;
    }

    protected void rudeTimerSubtraction(float deltaTime)
    {
        if (stateMachine.RudeTimer > 0)
        {
            stateMachine.RudeTimer -= deltaTime;
            stateMachine.Timer.text = ((int)(stateMachine.RudeTimer)).ToString();
        }
        else
        {
            EndRudeTimer();
        }
    }

    private void EndRudeTimer()
    {
        stateMachine.Timer.text = "";
    }
}