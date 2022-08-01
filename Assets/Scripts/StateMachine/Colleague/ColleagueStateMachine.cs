using System;
using System.Collections;
using System.Collections.Generic;
using Pv.Unity;
using UnityEngine;

public class ColleagueStateMachine : StateMachine
{
    [field: SerializeField] public float PlayerListenRange { get; private set; }
    public Transform Player { get; private set; }
    [field: SerializeField] public GameObject Target { get; private set; }

    public Camera playerHead;


    public bool _isProcessing;
    public RhinoManager _rhinoManager;

    private const string
        ACCESS_KEY =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

    private void Start()
    {
        _rhinoManager = RhinoManager.Create(ACCESS_KEY, GetContextPath(), inferenceCallback);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHead = Camera.main;

        SwitchState(new ColleagueWorkingState(this));
    }

    void inferenceCallback(Inference inference)
    {
        if (inference.IsUnderstood)
        {
            if (inference.Intent == "Someone_in_my_office")
            {
                Debug.Log("Someone in my office");
                SwitchState(new ColleagueTalkingState(this, 1));
            }
            else if (inference.Intent == "Seen_my_file")
            {
                Debug.Log("Seen my file");
            }
        }
        else
        {
            Debug.Log("Didn't understand the command.\n");
            SwitchState(new ColleagueTalkingState(this, 3));
        }

        _isProcessing = false;
    }

    private static string GetContextPath()
    {
        return "E:/UnityProjects/Picovoice/Assets/StreamingAssets/contexts/windows/colleague_windows.rhn";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerListenRange);
    }
}