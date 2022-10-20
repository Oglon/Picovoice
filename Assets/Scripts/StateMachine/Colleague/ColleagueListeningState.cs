using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColleagueListeningState : ColleagueBaseState
{
    public ColleagueListeningState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.gameObject.CompareTag($"Boss"))
        {
            if (stateMachine.ObjectiveHandler.getCurrentIndex() == 12)
            {
                var sceneName = SceneManager.GetActiveScene().name;

                if (sceneName.Contains("Office Level 1"))
                {
                    ColleagueStateMachine.delta = 10f;
                    stateMachine.SubtitlePanel.SetActive(true);
                    stateMachine.NameAnimatorPlayer.ShowText("Boss");
                    stateMachine.DialogueAnimatorPlayer.ShowText(
                        "You want the dor code, right?.");
                }
            }
        }

        stateMachine.Listening();
        stateMachine.Sprite.sprite = stateMachine.Ear;
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
            if (stateMachine.gameObject.tag == "Colleague")
            {
                var lookPos = stateMachine.MainTarget.transform.position - stateMachine.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                stateMachine.transform.rotation =
                    Quaternion.Slerp(stateMachine.transform.rotation, rotation, Time.deltaTime);
            }

            stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}