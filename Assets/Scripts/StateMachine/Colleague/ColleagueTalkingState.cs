using Pv.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ColleagueTalkingState : ColleagueBaseState
{
    public ColleagueTalkingState(ColleagueStateMachine stateMachine, Inference Inference) : base(stateMachine)
    {
        inferenceTalking(Inference);
    }

    public override void Enter()
    {
        stateMachine.Sprite.sprite = stateMachine.Speech;
    }

    public override void Tick(float deltaTime)
    {
        rudeTimerSubtraction(deltaTime);
        if (stateMachine.uiManager.delta <= 0)
        {
            if (IsInTalkingRange())
            {
                stateMachine.SwitchState(new ColleagueListeningState(stateMachine));
            }
            else
            {
                stateMachine.ObjectiveHandler.AudioBar.SetInactive();
                stateMachine.isProcessing = false;
                stateMachine.SwitchState(new ColleagueWorkingState(stateMachine));
            }
        }
    }

    public override void Exit()
    {
    }

    public void inferenceTalking(Inference inference)
    {
        if (!IsInTalkingRange())
            return;
        var sceneName = SceneManager.GetActiveScene().name;

        DialogueResponse dialogueResponse;

        if (stateMachine.Sensitive)
        {
            if (AudioBar.maxVolume >= 7)
            {
                dialogueResponse = stateMachine.CharacterResponse.GetVolumeResponse();
            }
            else
            {
                dialogueResponse = stateMachine.CharacterResponse.GetResponse(inference,
                    stateMachine.Sensitive,
                    stateMachine.rudeIncidents, stateMachine.RudeTimer);
            }

            AudioBar.maxVolume = 0;
        }
        else
        {
            dialogueResponse = stateMachine.CharacterResponse.GetResponse(inference,
                stateMachine.Sensitive,
                stateMachine.rudeIncidents, stateMachine.RudeTimer);
        }

        Debug.Log(dialogueResponse);

        if (dialogueResponse.excuse)
        {
            ReduceRudeTimer();
        }

        if (dialogueResponse.rude && stateMachine.Sensitive)
        {
            startRudeTimer();
        }

        if (dialogueResponse.CodeType != Code.Empty)
        {
            Debug.Log(dialogueResponse);
            SpawnGameObject(dialogueResponse.CodeType, stateMachine.ColleagueType);
        }

        stateMachine.AudioSource.clip = dialogueResponse.audioClip;
        stateMachine.AudioSource.Play();
        stateMachine.uiManager.GetDialogue(dialogueResponse);


        stateMachine.isProcessing = false;
    }

    private void SpawnGameObject(Code code, ColleagueType colleagueType)
    {
        if (code == Code.Documents)
        {
            GameObject DocumentA = GameObject.Find("Document_A");
            GameObject DocumentB = GameObject.Find("Document_B");
            GameObject DocumentC = GameObject.Find("Document_C");

            Outline outlineA = DocumentA.GetComponent<Outline>();
            Outline outlineB = DocumentB.GetComponent<Outline>();
            Outline outlineC = DocumentC.GetComponent<Outline>();

            outlineA.OutlineMode = Outline.Mode.OutlineAll;
            outlineB.OutlineMode = Outline.Mode.OutlineAll;
            outlineC.OutlineMode = Outline.Mode.OutlineAll;

            GameObject CollectibleSpawnerA = GameObject.Find("Collectible_Spawner_A");
            GameObject CollectibleSpawnerB = GameObject.Find("Collectible_Spawner_B");
            GameObject CollectibleSpawnerC = GameObject.Find("Collectible_Spawner_C");

            DocumentA.transform.position = CollectibleSpawnerA.transform.position;
            DocumentB.transform.position = CollectibleSpawnerB.transform.position;
            DocumentC.transform.position = CollectibleSpawnerC.transform.position;
            return;
        }

        if (code == Code.Donut)
        {
            GameObject Donut = GameObject.Find("Donut");
            GameObject DonutSpawner = GameObject.Find("Donut_Spawner");

            Outline outlineDonut = Donut.GetComponent<Outline>();

            outlineDonut.OutlineMode = Outline.Mode.OutlineAll;

            Donut.transform.position = DonutSpawner.transform.position;
            return;
        }

        if (code == Code.File)
        {
            GameObject File = GameObject.Find("File");
            GameObject FileSpawner = GameObject.Find("File_Spawner");

            Outline outlineFile = File.GetComponent<Outline>();
            outlineFile.OutlineMode = Outline.Mode.OutlineAll;

            File.transform.position = FileSpawner.transform.position;
            return;
        }

        if (code == Code.Card)
        {
            GameObject Keycard = GameObject.Find("Keycard");
            GameObject KeycardSpawner = GameObject.Find("Spawner_Keycard");

            Outline outlineFile = Keycard.GetComponent<Outline>();
            outlineFile.OutlineMode = Outline.Mode.OutlineAll;

            Keycard.transform.position = KeycardSpawner.transform.position;
            return;
        }

        if (code == Code.Phone)
        {
            GameObject Phone = GameObject.Find("Phone");
            GameObject PhoneSpawner = GameObject.Find("Spawner_Phone");

            Outline outlinePhone = Phone.GetComponent<Outline>();
            outlinePhone.OutlineMode = Outline.Mode.OutlineAll;

            Phone.transform.position = PhoneSpawner.transform.position;
            return;
        }

        if (code == Code.Prize)
        {
            GameObject Prize = GameObject.Find("Prize");
            GameObject PrizeSpawner = GameObject.Find("Spawner_Prize");

            Outline outlinePhone = Prize.GetComponent<Outline>();
            outlinePhone.OutlineMode = Outline.Mode.OutlineAll;

            Prize.transform.position = PrizeSpawner.transform.position;
            return;
        }

        GameObject place;
        if (colleagueType == ColleagueType.Intern)
        {
            place = GameObject.Find("Spawner_Intern");
        }
        else if (colleagueType == ColleagueType.Colleague)
        {
            place = GameObject.Find("Spawner_Colleague");
        }
        else
        {
            place = GameObject.Find("Spawner_Boss");
        }

        string stringCode = code + "Code";

        GameObject gameObject = GameObject.Find(stringCode);

        Outline outlineCode = gameObject.GetComponent<Outline>();
        outlineCode.OutlineMode = Outline.Mode.OutlineAll;

        Debug.Log(stringCode);
        gameObject.transform.position = place.transform.position;
    }

    private void startRudeTimer()
    {
        stateMachine.rudeIncidents++;
        stateMachine.RudeTimer = stateMachine.rudeIncidents * 30;
        stateMachine.Timer.text = ((int)(stateMachine.RudeTimer)).ToString();
    }

    private void ReduceRudeTimer()
    {
        stateMachine.RudeTimer -= 30f;
    }
}