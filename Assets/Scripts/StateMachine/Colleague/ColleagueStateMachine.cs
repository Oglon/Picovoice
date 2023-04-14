using Pv.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColleagueStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; private set; }
    [field: SerializeField] public ResponseScript CharacterResponse { get; set; }
    [field: SerializeField] public AudioLoudnessDetection Loudness { get; private set; }
    public Picovoice Picovoice { get; private set; }
    [field: SerializeField] public bool Sensitive { get; private set; }
    [field: SerializeField] public float RudeTimer { get; set; }
    [field: SerializeField] public int rudeIncidents = 0;


    [field: SerializeField] public MicrophoneVisual MicrophoneVisual { get; private set; }
    public UIManager uiManager;

    public AudioSource AudioSource;

    public Sprite Ear;
    public Sprite Speech;
    public Sprite Working;

    public ColleagueType ColleagueType;

    [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
    public Camera PlayerHead { get; private set; }
    [field: SerializeField] public TextMeshPro Timer { get; private set; }
    public static float delta;
    public static float endCounter;

    public bool isProcessing;

    private void Start()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
        Picovoice = GameObject.Find("Picovoice").GetComponent<Picovoice>();

        Ear = Resources.Load<Sprite>("Ear");
        Speech = Resources.Load<Sprite>("Speech");
        Working = Resources.Load<Sprite>("Working");

        uiManager = GameObject.Find("Dialogue Box").GetComponent<UIManager>();

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHead = Camera.main;
        ObjectiveHandler = Player.GetComponent<ObjectiveHandler>();


        SwitchState(new ColleagueWorkingState(this));

        Sensitive = SceneManager.GetActiveScene().name.Contains("Sensitive");

        Sprite.sprite = Working;

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


    public void ToggleProcessing()
    {
        isProcessing = !isProcessing;
    }
}