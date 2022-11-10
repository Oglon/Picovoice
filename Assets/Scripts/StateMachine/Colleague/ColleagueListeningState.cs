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
        if (SceneManager.GetActiveScene().name.Contains("Sensitive"))
        {
            stateMachine.ObjectiveHandler.Slider.SetActive(true);
        }

        if (stateMachine.gameObject.CompareTag($"Boss"))
        {
            if (stateMachine.ObjectiveHandler.getCurrentIndex() == 12)
            {
                var sceneName = SceneManager.GetActiveScene().name;

                if (sceneName.Contains("Office Level 1"))
                {
                    StringPass("You want the door code, right?");
                }
            }
        }

        stateMachine.Listening();
        stateMachine.Sprite.sprite = stateMachine.Ear;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Subtitles();

        if (stateMachine.Loudness.returnValue >= 0.5f && SceneManager.GetActiveScene().name.Contains("Sensitive"))
        {
            stateMachine.SwitchState(new ColleagueTalkingState(stateMachine));
        }

        Debug.Log(stateMachine.Loudness.returnValue);

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

    private void StringPass(string response)
    {
        stateMachine.DialogueAnimatorPlayer.ShowText(response);
        HandleAudio(response);
    }

    private void PlayAudio(AudioClip audioClip)
    {
        stateMachine.AudioSource.clip = audioClip;
        stateMachine.AudioSource.Play();
    }

    private AudioClip GetAudioClip(string title)
    {
        var str = title;
        var charsToRemove = new string[] { "!", "?" };
        foreach (var c in charsToRemove)
        {
            str = str.Replace(c, string.Empty);
        }

        if (str.EndsWith("."))
        {
            str = str.Remove(str.Length - 1);
        }

        Debug.Log(str);

        string clipPath = "Audio/" + stateMachine.ColleagueType.ToString() + "/";
        clipPath = clipPath + str;

        AudioClip clip = Resources.Load<AudioClip>(clipPath);
        return clip;
    }

    private void HandleAudio(string title)
    {
        AudioClip audioClip = GetAudioClip(title);
        float length;
        length = audioClip.length * 3;
        ColleagueStateMachine.delta = length;
        PlayAudio(audioClip);
    }
}