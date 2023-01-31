using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;

    public static bool GameIsPaused = false;

    [field: SerializeField] public StarterAssetsInputs Inputs;
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler;
    [field: SerializeField] public Picovoice Picovoice;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Inputs.gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            return;
        Inputs.gamePaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        PauseMenuUI.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Restart()
    {
        Picovoice.Restart();
        
        string Quest = "";

        if (SceneManager.GetActiveScene().name.Contains("1"))
        {
            Quest += "One: " + ObjectiveHandler._currentQuest.questIndex;
        }
        else
        {
            Quest += "Two: " + ObjectiveHandler._currentQuest.questIndex;
        }

        PlayerPrefs.SetString("Quest", Quest);
        Resume();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}