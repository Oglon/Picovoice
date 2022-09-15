using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueTalkingState : ColleagueBaseState
{
    private string _colleague;
    private int _response;

    Vector3 computer = new Vector3(0.7f, 1.2f, -0.16f);

    public ColleagueTalkingState(ColleagueStateMachine stateMachine, int response) : base(stateMachine)
    {
        this._response = response;
    }

    public override void Enter()
    {
        _colleague = stateMachine.gameObject.tag;
        Answer();
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();

        stateMachine.Target.transform.position = stateMachine.PlayerHead.transform.position;
    }

    public override void Exit()
    {
    }

    private void Answer()
    {
        if (_colleague == "Intern")
        {
            Debug.Log("Intern");
            if (_response == 1)
            {
                Debug.Log("1");
            }
        }

        if (_colleague == "Colleague")
        {
        }

        if (_colleague == "Boss")
        {
        }
    }
}