using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioBar : MonoBehaviour
{
    [field: SerializeField] public Image Audio1 { get; private set; }
    [field: SerializeField] public Image Audio2 { get; private set; }
    [field: SerializeField] public Image Audio3 { get; private set; }
    [field: SerializeField] public Image Audio4 { get; private set; }
    [field: SerializeField] public Image Audio5 { get; private set; }
    [field: SerializeField] public Image Audio6 { get; private set; }
    [field: SerializeField] public Image Audio7 { get; private set; }
    [field: SerializeField] public Image Audio8 { get; private set; }
    [field: SerializeField] public Image Audio9 { get; private set; }
    [field: SerializeField] public Image Audio10 { get; private set; }

    public static float MinVolume = 0;
    public static float MaxVolume = 1;

    public bool isActive;

    List<Image> images = new List<Image>();

    public AudioLoudnessDetection detector;

    public AudioSource Source;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    public static int maxVolume = 0;

    // Start is called before the first frame update
    void Start()
    {
        images.Add(Audio1);
        images.Add(Audio2);
        images.Add(Audio3);
        images.Add(Audio4);
        images.Add(Audio5);
        images.Add(Audio6);
        images.Add(Audio7);
        images.Add(Audio8);
        images.Add(Audio9);
        images.Add(Audio10);

        foreach (var image in images)
        {
            Color imageColor = image.color;
            imageColor.a = 0.4f;
            image.color = imageColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float volume = 0;
        float volumeStep = (MaxVolume - MinVolume) / 10;

        ActivateBar(VisualizeAudio(GetLoudness()));
    }


    private float GetLoudness()
    {
        return detector.GetLoudnessFromMicrophone();
    }

    public void SetActive()
    {
        isActive = true;
    }

    public void SetInactive()
    {
        isActive = false;
    }

    private int VisualizeAudio(float volume)
    {
        float step = (MaxVolume - MinVolume) / 10;
        return (int)((volume - MinVolume) / step);
    }

    private void ActivateBar(int index)
    {
        for (int i = 0; i < images.Count; i++)
        {
            Image image = images[i];
            Color imageColor = image.color;
            if (i > index)
            {
                imageColor.a = .4f;
                image.color = imageColor;
            }
            else
            {
                imageColor.a = 1f;
                image.color = imageColor;
                if (i > maxVolume)
                {
                    maxVolume = i;
                }
            }
        }
    }

    public float getMaxVolume()
    {
        if (PlayerPrefs.GetFloat("Max") > 0)
        {
            return PlayerPrefs.GetFloat("Max");
        }

        return 100;
    }

    public float getMinVolume()
    {
        if (PlayerPrefs.GetFloat("Min") > 0)
        {
            return PlayerPrefs.GetFloat("Min");
        }

        return 100;
    }

    public void SetMaxVolume(float volume)
    {
        MaxVolume = volume;
        PlayerPrefs.SetFloat("Max", volume);
    }

    public void SetMinVolume(float volume)
    {
        MinVolume = volume;
        PlayerPrefs.SetFloat("Min", volume);
    }
}