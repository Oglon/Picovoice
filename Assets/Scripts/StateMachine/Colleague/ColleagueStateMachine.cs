using Febucci.UI;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ColleagueStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; private set; }
    [field: SerializeField] public AudioLoudnessDetection Loudness { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer TextAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer NameAnimatorPlayer { get; private set; }
    [field: SerializeField] public Picovoice Picovoice { get; private set; }
    [field: SerializeField] public GameObject SubtitlePanel { get; private set; }
    [field: SerializeField] public GameObject NamePanel { get; private set; }
    [field: SerializeField] public bool Sensitive { get; private set; }

    [field: SerializeField] public Sprite Ear { get; private set; }
    [field: SerializeField] public Sprite Speech { get; private set; }
    [field: SerializeField] public Sprite Working { get; private set; }

    [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
    public Camera PlayerHead { get; private set; }

    public static float delta;

    public bool isProcessing;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHead = Camera.main;

        TextAnimatorPlayer.ShowText("");

        SwitchState(new ColleagueWorkingState(this));

        Sensitive = SceneManager.GetActiveScene().name.Contains("Sensitive");
        Sprite.sprite = Working;
    }

    public void inferenceReaction(Inference inference)
    {
        SwitchState(new ColleagueTalkingState(this, inference));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerListenRange);
    }

    private bool IsInTalkingRange()
    {
        float playerDistanceSqr =
            (Player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= PlayerListenRange * PlayerListenRange;
    }
    
    private void StartProcessing()
    {
        if (isProcessing)
            return;

        ToggleProcessing();
        Picovoice.setStateMachine(this);
        Picovoice.RhinoProcessing();
    }

    public void Listening()
    {
        isProcessing = false;
        StartProcessing();
    }

    public void Subtitles()
    {
        switch (delta)
        {
            case > 0:
                delta -= Time.deltaTime;
                break;
            case <= 0:
                TextAnimatorPlayer.ShowText("");
                SubtitlePanel.SetActive(false);
                NamePanel.SetActive(false);
                break;
        }
    }
    
    public void ToggleProcessing()
    {
        isProcessing = !isProcessing;
    }
}