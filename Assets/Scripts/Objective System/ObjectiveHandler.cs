using UnityEngine;
using UnityEngine.SceneManagement;


public class ObjectiveHandler : MonoBehaviour
{
    public AudioBar AudioBar;
    public Quest _currentQuest;
    [field: SerializeField] public Quest startQuest;

    [field: SerializeField] public QuestHolder QuestHolder;

    private float endTimer = 5f;
    private bool end = false;

    public UIManager uiManager;

    public void FinishQuest()
    {
        _currentQuest.OnFollowingQuest -= FinishQuest;
        _currentQuest.OnProgress -= FinishObjective;
        _currentQuest = _currentQuest.GetFollowUp();
        _currentQuest.currentObjective = _currentQuest.objectives[0];
        _currentQuest.OnFollowingQuest += FinishQuest;
        _currentQuest.OnProgress += FinishObjective;
        Debug.Log(_currentQuest.currentObjective.name);
        uiManager.UpdateObjective(_currentQuest.currentObjective.objectiveDesc);
    }

    public void FinishObjective()
    {
        _currentQuest.currentObjective =
            _currentQuest.objectives[_currentQuest.objectives.IndexOf(_currentQuest.currentObjective) + 1];
        uiManager.UpdateObjective(_currentQuest.currentObjective.objectiveDesc);

        if (_currentQuest.GetCurrentObjective().getDescription().Contains("Level Complete!"))
        {
            end = true;
        }
    }


    void Start()
    {
        _currentQuest = getQuestFromHolder(PlayerPrefs.GetString("Quest"));
        PlayerPrefs.SetString("Quest", "");
        _currentQuest.OnFollowingQuest += FinishQuest;
        _currentQuest.OnProgress += FinishObjective;
        uiManager.UpdateObjective(_currentQuest.currentObjective.objectiveDesc);
    }

    private Quest getQuestFromHolder(string Quest)
    {
        if (Quest.Contains("One"))
        {
            if (Quest.Contains("1"))
            {
                return QuestHolder.Q101;
            }

            if (Quest.Contains("2"))
            {
                return QuestHolder.Q102;
            }

            if (Quest.Contains("3"))
            {
                return QuestHolder.Q103;
            }

            if (Quest.Contains("4"))
            {
                return QuestHolder.Q104;
            }

            if (Quest.Contains("5"))
            {
                return QuestHolder.Q105;
            }

            return QuestHolder.Q101;
        }
        else
        {
            if (Quest.Contains("Two"))
            {
                if (Quest.Contains("1"))
                {
                    return QuestHolder.Q201;
                }

                if (Quest.Contains("2"))
                {
                    return QuestHolder.Q202;
                }

                if (Quest.Contains("3"))
                {
                    return QuestHolder.Q203;
                }

                if (Quest.Contains("4"))
                {
                    return QuestHolder.Q204;
                }

                return QuestHolder.Q201;
            }

            if (SceneManager.GetActiveScene().name.Contains("1"))
            {
                return QuestHolder.Q101;
            }

            return QuestHolder.Q201;
        }
    }

    public Quest GetCurrentQuest()
    {
        return _currentQuest;
    }

    private void Update()
    {
        if (end)
        {
            endTimer -= Time.deltaTime;
        }

        if (endTimer <= 0)
        {
            uiManager.End();
            endTimer = 100f;
        }

        if (Input.GetKey(KeyCode.H))
        {
            uiManager.ShowHint(_currentQuest.currentObjective.objectiveHint);
        }
        else if (!Input.GetKey(KeyCode.H))
        {
            uiManager.HideHint();
        }
    }
}