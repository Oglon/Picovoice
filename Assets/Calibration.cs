using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    private bool calibrating;
    private bool testing;
    [field: SerializeField] public AudioLoudnessDetection LoudnessDetection { get; set; }
    [field: SerializeField] public TextMeshProUGUI TMP { get; set; }
    [field: SerializeField] public TextMeshProUGUI CountdownTMP { get; set; }

    [field: SerializeField] public Button CalibrateButton { get; set; }

    [field: SerializeField] public Button TestButton { get; set; }
    [field: SerializeField] public AudioBar AudioBar { get; set; }


    private float delta = 4;
    private float volume;

    int state = 0;

    private void Start()
    {
        CountdownTMP.enabled = false;
        AudioBar.enabled = false;
        AudioBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (testing)
        {
        }

        if (calibrating)
        {
            switch (state)
            {
                case 0:
                    TMP.text = "Scream or talk very loudly in:";
                    CountdownTMP.text = Countdown().ToString();
                    break;
                case 1:
                    TMP.text = "Scream or talk very loudly NOW!";
                    delta = 4;
                    state++;
                    break;
                case 2:
                    getHighestVolume();
                    CountdownTMP.text = Countdown().ToString();
                    break;
                case 3:
                    AudioBar.SetMaxVolume(volume);
                    volume = 0f;
                    state++;
                    delta = 4;
                    break;
                case 4:
                    TMP.text = "Be as silent as possible in:";
                    CountdownTMP.text = Countdown().ToString();
                    break;
                case 5:
                    TMP.text = "Be as silent as possible NOW!";
                    delta = 4;
                    state++;
                    break;
                case 6:
                    getHighestVolume();
                    CountdownTMP.text = Countdown().ToString();
                    break;
                case 7:
                    AudioBar.SetMinVolume(volume);
                    state++;
                    delta = 4;
                    break;
                case 8:
                    TMP.text = "Calibration successful!";
                    CountdownTMP.text = Countdown().ToString();
                    break;
                case 9:
                    delta = 4f;
                    calibrating = false;
                    break;
            }
        }
        else
        {
            TMP.text = "Click on calibrate to start calibrating the microphone";
            CountdownTMP.enabled = false;
        }
    }

    public void StartCalibration()
    {
        AudioBar.gameObject.SetActive(false);
        TMP.enabled = true;
        calibrating = true;
    }

    private int Countdown()
    {
        if (!CountdownTMP.enabled)
        {
            CountdownTMP.enabled = true;
        }

        if (delta <= 0)
        {
            state++;
        }

        return (int)(delta -= Time.deltaTime);
    }

    private void getHighestVolume()
    {
        float tempVol = LoudnessDetection.GetLoudnessFromMicrophone();
        Debug.Log(tempVol);
        if (!(tempVol > volume)) return;
        volume = tempVol;
    }

    public void Test()
    {
        testing = true;
        AudioBar.gameObject.SetActive(true);
        AudioBar.enabled = true;
        TMP.enabled = false;
        AudioBar.SetActive();
    }
}