using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    public float returnValue;

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
        if (SceneManager.GetActiveScene().name.Contains("StartMenu"))
        {
            return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
        }

        return returnValue;
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
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


        returnValue = totalLoudness / sampleWindow;
        // if (_returnValue <= 0.1f)
        // {
        //     _returnValue = 0;
        // }

        return returnValue;
    }
}