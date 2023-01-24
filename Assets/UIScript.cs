using UnityEngine;

public class UIScript : MonoBehaviour
{
    [field: SerializeField] public GameObject Panel;
    [field: SerializeField] public GameObject Notice;
    [field: SerializeField] public GameObject LevelSelectionPanel;
    [field: SerializeField] public GameObject CalibrationPanel;
    [field: SerializeField] public GameObject InstructionPanel;
    [field: SerializeField] public GameObject GameTitle;
    [field: SerializeField] public GameObject AudioLoudnessDetection;

    private int MenuPart;

    private void Start()
    {
        Panel.SetActive(true);
        GameTitle.SetActive(true);
        AudioLoudnessDetection.SetActive(false);
        Notice.SetActive(false);
        LevelSelectionPanel.SetActive(false);
        CalibrationPanel.SetActive(false);
        InstructionPanel.SetActive(false);
        MenuPart = 0;
    }

    private void Update()
    {
        MicrophonePanelActivation(Microphone.devices.Length <= 0);
        if (Input.GetKey(KeyCode.Escape))
        {
            LevelSelectionPanel.SetActive(false);
            CalibrationPanel.SetActive(false);
            InstructionPanel.SetActive(false);
        }
    }

    private void DeactivateMainMenu()
    {
        Panel.SetActive(false);
    }

    private void ActivateMainMenu()
    {
        Panel.SetActive(true);
    }

    public void LevelSelection()
    {
        DeactivateAllPanels();
        MenuPart = 1;
        LevelSelectionPanel.SetActive(true);
        DeactivateMainMenu();
    }

    private void MicrophonePanelActivation(bool micState)
    {
        GameTitle.SetActive(!micState);
        Panel.SetActive(!micState);
        Notice.SetActive(micState);
    }

    public void MicrophoneCalibration()
    {
        DeactivateAllPanels();
        CalibrationPanel.SetActive(true);
        AudioLoudnessDetection.SetActive(true);
    }

    public void Instructions()
    {
        DeactivateAllPanels();
        InstructionPanel.SetActive(true);
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    private void DeactivateAllPanels()
    {
        Notice.SetActive(false);
        LevelSelectionPanel.SetActive(false);
        CalibrationPanel.SetActive(false);
        InstructionPanel.SetActive(false);
    }
}