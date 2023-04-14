using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    public float returnValue;
    private string microphoneName;

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        if (SceneManager.GetActiveScene().name.Contains("StartMenu"))
        {
            Debug.Log("One");
            if (microphoneClip == null)
            {
                Debug.Log("Two");
                MicrophoneToAudioClip();
            }

            return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
        }

        return returnValue;
    }

    public void EndClip()
    {
        Microphone.End(microphoneName);
        microphoneClip = null;
    }

    public float GetSilenceFromMicrophone()
    {
        return GetSilenceFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }

        float rms = Mathf.Sqrt(sum / samples.Length);

        Debug.Log("RMS: "+rms);

        
        return rms;
    }

    public float GetSilenceFromAudioClip(int clipPosition, AudioClip clip)
    {
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }

        float rms = Mathf.Sqrt(sum / samples.Length);

        return rms;
    }
}