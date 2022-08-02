using System.Collections;
using System.Collections.Generic;
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
}