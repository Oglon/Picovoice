using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFromMicrophone : MonoBehaviour
{
    public AudioLoudnessDetection detector;

    public Slider slider;

    public float threshold = 0.1f;

    public float loudness;

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        loudness = detector.GetLoudnessFromMicrophone() * 2;

        if (loudness < threshold)
            loudness = 0;

        slider.value = loudness;
    }
}