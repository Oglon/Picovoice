using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFromMicrophone : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    
    public Slider slider;

    public float threshold = 0.1f;

    public float loudness =0;
    public float currentVolume;

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentVolume = detector.GetLoudnessFromMicrophone() * 2;

        loudness = Mathf.MoveTowards(loudness, currentVolume, Time.deltaTime);
        
        slider.value = loudness;
    }
}