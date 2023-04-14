using System.IO;
using Febucci.UI;
using Pv.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaristaStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public AudioLoudnessDetection Loudness { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer TextAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer NameAnimatorPlayer { get; private set; }
    [field: SerializeField] public GameObject SubtitlePanel { get; private set; }
    [field: SerializeField] public GameObject NamePanel { get; private set; }
    [field: SerializeField] public bool Sensitive { get; private set; }
    [field: SerializeField] public AudioBar AudioBar { get; private set; }

    [field: SerializeField] public Sprite Ear { get; private set; }
    [field: SerializeField] public Sprite Speech { get; private set; }
    [field: SerializeField] public Sprite Working { get; private set; }
    [field: SerializeField] public SpriteRenderer Sprite { get; private set; }

    [field: SerializeField] public PickUpController PickUpController { get; private set; }
    [field: SerializeField] public TextMeshPro Timer { get; private set; }
    public Camera PlayerHead { get; private set; }

    public static float delta;

    public bool isProcessing;

    private RhinoManager _rhinoManager;

    private int count;

    private GameObject coffee;

    public UIManager uiManager;

    public AudioSource AudioSource;

    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public MicrophoneVisual MicrophoneVisual { get; private set; }

    private const string
        AccessKey =
            "QJwhoDsaCFxFuSJ28NORV0CXYtWeu99Wb8f9MrXv7ZlKCY2z8490Nw==";

    [field: SerializeField] public DialogueResponse Guide1 { get; private set; }
    [field: SerializeField] public DialogueResponse Barista2 { get; private set; }
    [field: SerializeField] public DialogueResponse Guide3 { get; private set; }
    [field: SerializeField] public DialogueResponse Barista4 { get; private set; }
    [field: SerializeField] public DialogueResponse Guide5 { get; private set; }
    [field: SerializeField] public DialogueResponse Barista6 { get; private set; }
    [field: SerializeField] public DialogueResponse Guide7 { get; private set; }
    [field: SerializeField] public DialogueResponse Barista8 { get; private set; }
    [field: SerializeField] public DialogueResponse Guide9 { get; private set; }
    [field: SerializeField] public DialogueResponse Guide10 { get; private set; }
    [field: SerializeField] public DialogueResponse Guide11 { get; private set; }

    private void Start()
    {
        _rhinoManager = RhinoManager.Create(AccessKey, GetContextPath(), InferenceCallback);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHead = Camera.main;
        Sprite.sprite = Working;

        Icon.sprite = Resources.Load<Sprite>("Coffee");

        Icon.gameObject.SetActive(false);

        AudioBar.gameObject.SetActive(false);

        coffee = GameObject.FindWithTag("Coffee");
        coffee.SetActive(false);

        uiManager = GameObject.Find("Dialogue Box").GetComponent<UIManager>();
        uiManager.HintTMP.text = "You don't need a Hint for this part.";
        uiManager.UpdateObjective("Grab a morning coffee");
        count = 0;
        Timer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            uiManager.ShowHint("Well done!");
            if (count == 14)
            {
                count++;
            }
        }
        else if (!Input.GetKey(KeyCode.H))
        {
            uiManager.HideHint();
        }

        if (delta > 0)
        {
            delta -= Time.deltaTime;
        }
        else
        {
            Order();
        }
    }

    private void Order()
    {
        switch (count)
        {
            case 0:
                PlayAudio(Guide1);
                delta = Guide1.audioClip.length - 2f;
                count++;
                break;
            case 1:
                Sprite.sprite = Ear;
                MicrophoneVisual.IsActive();
                Listening();
                break;
            case 2:
                transform.localRotation *= Quaternion.Euler(0, 180, 0);
                PlayAudio(Barista2);
                delta = Barista2.audioClip.length;
                count++;
                break;
            case 3:
                PlayAudio(Guide3);
                delta = Guide3.audioClip.length;
                count++;
                break;
            case 4:
                Sprite.sprite = Ear;
                MicrophoneVisual.IsActive();
                Listening();
                break;
            case 5:
                PlayAudio(Barista4);
                delta = Barista4.audioClip.length;
                count++;
                break;
            case 6:
                PlayAudio(Guide5);
                delta = Guide5.audioClip.length;
                count++;
                break;
            case 7:
                Sprite.sprite = Ear;
                MicrophoneVisual.IsActive();
                Listening();
                break;
            case 8:
                PlayAudio(Barista6);
                delta = Barista6.audioClip.length;
                count++;
                Timer.gameObject.SetActive(true);
                break;
            case 9:
                PlayAudio(Guide7);
                delta = Guide7.audioClip.length;
                count++;
                break;
            case 10:
                Sprite.sprite = Ear;
                MicrophoneVisual.IsActive();
                Listening();
                break;
            case 11:
                Timer.gameObject.SetActive(false);
                PlayAudio(Barista8);
                delta = Barista8.audioClip.length;
                coffee.SetActive(true);
                count++;
                break;
            case 12:
                AudioBar.gameObject.SetActive(true);
                PlayAudio(Guide9);
                delta = Guide9.audioClip.length;
                count++;
                break;
            case 13:
                PlayAudio(Guide10);
                delta = Guide10.audioClip.length;
                count++;
                break;
            case 15:
                PlayAudio(Guide11);
                delta = Guide11.audioClip.length;
                count++;
                break;
            case 16:
                Cursor.lockState = CursorLockMode.None;
                _rhinoManager.Delete();
                SceneManager.LoadScene("StartMenu");
                break;
        }
    }

    void InferenceCallback(Inference inference)
    {
        if (!IsInTalkingRange())
            return;

        SubtitlePanel.SetActive(true);


        if (inference.IsUnderstood)
        {
            if (inference.Intent == "Attention")
            {
                if (count == 1)
                {
                    count++;
                    Sprite.sprite = Speech;
                    MicrophoneVisual.IsInactive();
                }
            }
            else if (inference.Intent == "OneCoffee")
            {
                if (count == 4)
                {
                    count++;
                    Sprite.sprite = Speech;
                    MicrophoneVisual.IsInactive();
                }
            }
            else if (inference.Intent == "OneCoffeeFriendly")
            {
                if (count == 4)
                {
                    count++;
                    Sprite.sprite = Speech;
                    MicrophoneVisual.IsInactive();
                }
            }
            else if (inference.Intent == "Faster")
            {
                if (count == 7)
                {
                    count++;
                    Sprite.sprite = Speech;
                    MicrophoneVisual.IsInactive();
                }
            }
            else if (inference.Intent == "Sorry")
            {
                if (count == 10)
                {
                    count++;
                    Sprite.sprite = Speech;
                    MicrophoneVisual.IsInactive();
                }
            }
            else if (inference.Intent == "YouToo")
            {
            }

            isProcessing = false;
        }
    }

    private static string GetContextPath()
    {
        string srcPath = Path.Combine(Application.streamingAssetsPath, "contexts/windows/barista.rhn");
        return srcPath;
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

    private void ToggleProcessing()
    {
        isProcessing = !isProcessing;

        if (!isProcessing)
        {
            // SwitchState(new ColleagueWorkingState(this));
        }
    }

    public void Listening()
    {
        isProcessing = false;
        StartProcessing();
    }

    private void StartProcessing()
    {
        if (isProcessing)
            return;

        ToggleProcessing();
        _rhinoManager.Process();
    }

    private void PlayAudio(DialogueResponse response)
    {
        AudioSource.clip = response.audioClip;
        AudioSource.Play();
        uiManager.GetDialogue(response);
    }
}