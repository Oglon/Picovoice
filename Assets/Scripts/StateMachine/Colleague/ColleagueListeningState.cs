using UnityEngine;

public class ColleagueListeningState : ColleagueBaseState
{
    public ColleagueListeningState(ColleagueStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ObjectiveHandler.AudioBar.SetActive();
        stateMachine.Listening();
        stateMachine.Sprite.sprite = stateMachine.Ear;
        stateMachine.MicrophoneVisual.IsActive();
    }

    public override void Tick(float deltaTime)
    {
        rudeTimerSubtraction(deltaTime);
        
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

            stateMachine.ObjectiveHandler.AudioBar.SetInactive();
            stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.MicrophoneVisual.IsInactive();
    }

    private void StringPass(string response)
    {
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