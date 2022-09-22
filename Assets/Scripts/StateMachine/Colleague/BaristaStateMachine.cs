using Febucci.UI;
using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaristaStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; private set; }
    [field: SerializeField] public AudioLoudnessDetection Loudness { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer TextAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer NameAnimatorPlayer { get; private set; }
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

    private RhinoManager _rhinoManager;

    private int count;

    private GameObject coffee;

    private const string
        AccessKey =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

    private void Start()
    {
        _rhinoManager = RhinoManager.Create(AccessKey, GetContextPath(), InferenceCallback);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHead = Camera.main;
        Sprite.sprite = Working;

        coffee = GameObject.FindWithTag("Coffee");
        coffee.SetActive(false);
    }

    private void Update()
    {
        switch (delta)
        {
            case > 0:
                delta -= Time.deltaTime;
                break;
            case <= 0:
                switch (count)
                {
                    case 0:
                        delta = 6f;
                        NameAnimatorPlayer.ShowText("?");
                        TextAnimatorPlayer.ShowText(
                            "You probably want to order a coffee but the Barista hasn't noticed you, right? She is working as the Sprite indicates.");
                        count++;
                        break;
                    case 1:
                        count++;
                        delta = 4f;
                        TextAnimatorPlayer.ShowText(
                            "If you want her attention you should try saying \"Excuse me\"");
                        StartProcessing();
                        break;
                    case 3:
                        delta = 6f;
                        NameAnimatorPlayer.ShowText("?");
                        TextAnimatorPlayer.ShowText(
                            "People react to your politeness. For a polite order say \"Can I get a coffee, please?\"");
                        StartProcessing();
                        count++;
                        break;
                    case 5:
                        delta = 6f;
                        NameAnimatorPlayer.ShowText("?");
                        TextAnimatorPlayer.ShowText(
                            "Congratulations, you were polite and got your morning coffee!");
                        count++;
                        break;
                    case 6:
                        SceneManager.LoadScene("LevelSelection");

                        break;
                }

                break;
        }
    }

    void InferenceCallback(Inference inference)
    {
        if (!IsInTalkingRange())
            return;
        var sceneName = SceneManager.GetActiveScene().name;

        delta = 4f;
        SubtitlePanel.SetActive(true);


        if (inference.IsUnderstood)
        {
            if (inference.Intent == "Attention")
            {
                float degrees = 0;
                Vector3 to = new Vector3(0, degrees, 0);

                gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.rotation.eulerAngles, to, Time.deltaTime);

                Sprite.sprite = Speech;
                delta = 4f;
                NameAnimatorPlayer.ShowText("Barista");
                TextAnimatorPlayer.ShowText(
                    "Oh sorry, I didn't see you enter. What can I get you?");
                count++;
            }
            else if (inference.Intent == "OneCoffee")
            {
                Sprite.sprite = Speech;
                delta = 4f;
                NameAnimatorPlayer.ShowText("Barista");
                TextAnimatorPlayer.ShowText(
                    "Sure, one coffee coming right up. Here you go!");
                coffee.SetActive(true);
                count++;
                Debug.Log(count);
            }
            else if (inference.Intent == "OneCoffeeFriendly")
            {
                Sprite.sprite = Speech;
                delta = 4f;
                NameAnimatorPlayer.ShowText("Barista");
                TextAnimatorPlayer.ShowText(
                    "Sure, one coffee coming right up. Here you go!");
                coffee.SetActive(true);
                count++;
            }
            else if (inference.Intent == "YouToo")
            {
            }
            else
            {
                TextAnimatorPlayer.ShowText("I'm sorry but I have no idea what you are talking about.");
                ToggleProcessing();
            }


            // Loudness.gameObject.SetActive(false);

            isProcessing = false;
        }
    }

    private static string GetContextPath()
    {
        return "D:/Unity Projects/Picovoice/Assets/StreamingAssets/contexts/windows/barista.rhn";
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

    public void Subtitles()
    {
    }
}