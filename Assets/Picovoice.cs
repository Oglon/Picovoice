using Pv.Unity;
using UnityEngine;

public class Picovoice : MonoBehaviour
{
    private RhinoManager _rhinoManager;

    private Inference picoInference;

    private ColleagueStateMachine _stateMachine;
    
    private const string
        AccessKey =
            "LEXyhVN7pdElKZ0mRGtgdoPGPg8MzEN2Tj0QuA3LqQESAX+y6o5o8A==";

    // Start is called before the first frame update
    void Start()
    {
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
   
    private static string GetContextPath()
    {
        return "D:/Unity Projects/Picovoice/Assets/StreamingAssets/contexts/windows/colleague_windows.rhn";
    }
    
}
