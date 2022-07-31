using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Pv.Unity;
using UnityEngine.UI;
using System.Linq;

public class PicovoiceImplementation : MonoBehaviour
{
    private const string
        ACCESS_KEY =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A=="; // AccessKey obtained from Picovoice Console (https://picovoice.ai/console/)

    Button _startButton;
    Image[] _locationStates;
    Text _errorMessage;

    private bool _isProcessing;

    RhinoManager _rhinoManager;

    private static readonly string _contextPath;
    private static readonly string _platform;

    public AudioSource AudioSource = new AudioSource();
    private InverseTarget inverseTarget;

    static PicovoiceImplementation()
    {
        _platform = GetPlatform();
        _contextPath = GetContextPath();
    }

    void Start()
    {
        inverseTarget = GetComponentInChildren<InverseTarget>();

        try
        {
            _rhinoManager = RhinoManager.Create(ACCESS_KEY, _contextPath, OnInferenceResult,
                processErrorCallback: ErrorCallback);
        }
        catch (RhinoInvalidArgumentException)
        {
            Debug.Log("Make sure your access key '{ACCESS_KEY}' is a valid access key.");
        }
        catch (RhinoActivationException)
        {
            SetError("AccessKey activation error");
        }
        catch (RhinoActivationLimitException)
        {
            SetError("AccessKey reached its device limit");
        }
        catch (RhinoActivationRefusedException)
        {
            SetError("AccessKey refused");
        }
        catch (RhinoActivationThrottledException)
        {
            SetError("AccessKey has been throttled");
        }
        catch (RhinoException ex)
        {
            SetError("RhinoManager was unable to initialize: " + ex.Message);
        }
    }

    private void ToggleProcessing()
    {
        if (!_isProcessing)
        {
            StartProcessing();
        }
    }

    private void StartProcessing()
    {
        _rhinoManager.Process();
        _isProcessing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ToggleProcessing();
            Debug.Log("In Talking Range");
            inverseTarget.lookat = true;
        }
    }

    private void OnInferenceResult(Inference inference)
    {
        if (inference.IsUnderstood)
        {
            if (inference.Intent == "Someone_in_my_office")
            {
                AudioSource?.Play();
            }
            else if (inference.Intent == "Seen_my_file")
            {
            }
        }
        else
        {
            Debug.Log("Didn't understand the command.\n");
        }

        _isProcessing = false;
    }

    private void ErrorCallback(RhinoException e)
    {
        SetError(e.Message);
    }

    private void SetError(string message)
    {
        _errorMessage.text = message;
        _startButton.interactable = false;
    }

    void Update()
    {
    }

    void OnApplicationQuit()
    {
        if (_rhinoManager != null)
        {
            _rhinoManager.Delete();
        }
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
                throw new NotSupportedException(string.Format("Platform '{0}' not supported by Rhino Unity binding",
                    Application.platform));
        }
    }

    public static string GetContextPath()
    {
        // string fileName = string.Format("colleague_{0}.rhn", _platform);
        // string srcPath = Path.Combine(Application.streamingAssetsPath,
        //     string.Format("contexts/{0}/{1}", _platform, fileName));
        // return srcPath;
        return "E:/UnityProjects/Picovoice/Assets/StreamingAssets/contexts/windows/colleague_windows.rhn";
    }
}