using System;
using System.IO;
using Pv.Unity;
using UnityEngine;

public class Picovoice : MonoBehaviour
{
    private RhinoManager _rhinoManager;

    private Inference picoInference;

    private ColleagueStateMachine _stateMachine;

    private static readonly string _platform;

    private bool stop;

    private const string
        AccessKey =
            "7TDa4XH8Nbz8qOyz7aGD1mN6Fyp+JZEDdbalKpMI/MqR2lfHLzTLZw==";

    // Start is called before the first frame update
    void Start()
    {
        GetPlatform();
        _rhinoManager = RhinoManager.Create(AccessKey, GetContextPath(), InferenceCallback);
    }

    public void setStateMachine(ColleagueStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public Inference getInference()
    {
        return picoInference;
    }

    void InferenceCallback(Inference inference)
    {
        picoInference = inference;
        Debug.Log("Pico Inference " + inference.Intent);
        Debug.Log(_stateMachine);
        _stateMachine.inferenceReaction(picoInference);
    }

    public void RhinoProcessing()
    {
        _rhinoManager.Process();
    }

    private static string GetPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "windows";
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "mac";
            case RuntimePlatform.LinuxEditor:
            case RuntimePlatform.LinuxPlayer:
                return "linux";
            case RuntimePlatform.IPhonePlayer:
                return "ios";
            case RuntimePlatform.Android:
                return "android";
            default:
                throw new NotSupportedException(string.Format("Platform '{0}' not supported by Picovoice Unity binding",
                    Application.platform));
        }
    }

    public static string GetContextPath()
    {
        string srcPath = Path.Combine(Application.streamingAssetsPath,
            "contexts/" + GetPlatform() + "/colleague.rhn");
        return srcPath;
    }

    public void Restart()
    {
        _rhinoManager.Delete();
    }

    public void Delete()
    {
        stop = true;
        _rhinoManager.Delete();
    }
}