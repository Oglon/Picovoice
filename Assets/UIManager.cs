using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public GameObject DialogueBox { get; private set; }
    [field: SerializeField] public GameObject HintBox { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer DialogueAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer NameAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextAnimatorPlayer ObjectiveAnimatorPlayer { get; private set; }
    [field: SerializeField] public TextMeshProUGUI HintTMP { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    [field: SerializeField] public Image Icon { get; private set; }

    public float responseTime;
    public float delta;

    private string endMessage;

    private bool ending;
    private float countdown = 3f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("1"))
        {
            Icon.sprite = Resources.Load<Sprite>("File");
            endMessage = "Congratulations, you learned the location of your file.";
        }
        else
        {
            Icon.sprite = Resources.Load<Sprite>("Price");
            endMessage = "Congratulations! You won the Office Prize by helping everyone.";
        }

        Icon.enabled = false;

        DialogueAnimatorPlayer = GameObject.Find("Dialogue").GetComponent<TextAnimatorPlayer>();
        NameAnimatorPlayer = GameObject.Find("Name").GetComponent<TextAnimatorPlayer>();
        DialogueBox = GameObject.Find("Dialogue Box");

        ObjectiveAnimatorPlayer = GameObject.Find("Objective").GetComponent<TextAnimatorPlayer>();

        HintTMP = GameObject.Find("Hint").GetComponent<TextMeshProUGUI>();
        HintBox = GameObject.Find("Hint Box");
        HintBox.SetActive(false);
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
                DialogueBox.SetActive(false);
                break;
        }
    }

    public void GetDialogue(DialogueResponse response)
    {
        DialogueBox.SetActive(true);
        AudioSource.clip = response.audioClip;
        StartDialogue(response.audioClip.length);
        DialogueAnimatorPlayer.ShowText(response.text);
    }

    // Update is called once per frame
    void Update()
    {
        Subtitles();

        if (ending)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void StartDialogue(float clipLength)
    {
        delta = clipLength;
    }

    public void ShowHint(string hint)
    {
        HintBox.SetActive(true);
        HintTMP.text = hint;
    }

    public void HideHint()
    {
        HintBox.SetActive(false);
    }

    public void UpdateObjective(string objective)
    {
        ObjectiveAnimatorPlayer.ShowText(objective);
    }

    public void End()
    {
        Icon.enabled = true;
        DialogueBox.SetActive(true);
        DialogueAnimatorPlayer.ShowText(endMessage);
        ending = true;
    }
}