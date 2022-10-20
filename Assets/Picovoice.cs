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

    private const string
        AccessKey =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

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
        string srcPath = Path.Combine(Application.streamingAssetsPath, "contexts/windows/colleague_windows.rhn");
        return srcPath;
    }
    // return "D:/Unity Projects/Picovoice/Assets/StreamingAssets/contexts/windows/colleague_windows.rhn";
}