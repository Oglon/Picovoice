using Febucci.UI;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ColleagueStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; private set; }
    [field: SerializeField] public AudioLoudnessDetection Loudness { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer DialogueAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer NameAnimatorPlayer { get; private set; }
    [field: SerializeField] public Picovoice Picovoice { get; private set; }
    [field: SerializeField] public GameObject SubtitlePanel { get; private set; }
    [field: SerializeField] public GameObject Slider { get; private set; }
    [field: SerializeField] public bool Sensitive { get; private set; }

    public AudioSource AudioSource;

    public Sprite Ear;
    public Sprite Speech;
    public Sprite Working;

    public ColleagueType ColleagueType;

    [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
    public Camera PlayerHead { get; private set; }

    public static float delta;

    public bool isProcessing;

    private void Start()
    {
        DialogueAnimatorPlayer = GameObject.Find("Dialogue").GetComponent<TextAnimatorPlayer>();
        NameAnimatorPlayer = GameObject.Find("Name").GetComponent<TextAnimatorPlayer>();
        Slider = GameObject.Find("Slider");

        AudioSource = gameObject.GetComponent<AudioSource>();

        SubtitlePanel = GameObject.Find("Dialogue Box");

        Ear = Resources.Load<Sprite>("Ear");
        Speech = Resources.Load<Sprite>("Speech");
        Working = Resources.Load<Sprite>("Working");

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHead = Camera.main;
        ObjectiveHandler = Player.GetComponent<ObjectiveHandler>();

        DialogueAnimatorPlayer.ShowText("");

        SwitchState(new ColleagueWorkingState(this));

        Sensitive = SceneManager.GetActiveScene().name.Contains("Sensitive");
        Sprite.sprite = Working;

        if (Sensitive)
        {
            Slider.SetActive(true);
        }


        if (gameObject.CompareTag("Intern"))
        {
            ColleagueType = ColleagueType.Intern;
        }
        else if (gameObject.CompareTag("Colleague"))
        {
            ColleagueType = ColleagueType.Colleague;
        }
        else
        {
            ColleagueType = ColleagueType.Boss;
        }
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
                DialogueAnimatorPlayer.ShowText("");
                SubtitlePanel.SetActive(false);
                break;
        }
    }

    public void ToggleProcessing()
    {
        isProcessing = !isProcessing;
    }
}