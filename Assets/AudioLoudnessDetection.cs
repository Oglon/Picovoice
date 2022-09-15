using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    private float _returnValue;

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        Debug.Log("Call");
        int startposition = clipPosition - sampleWindow;

        if (startposition < 0)
            return 0;

        float[] wavedata = new float[sampleWindow];
        clip.GetData(wavedata, startposition);

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(wavedata[i]);
        }


        _returnValue = totalLoudness / sampleWindow;
        // if (_returnValue <= 0.1f)
        // {
        //     _returnValue = 0;
        // }

        return _returnValue;
    }
}