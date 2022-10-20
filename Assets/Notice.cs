using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{
    private GameObject Panel;

    // Start is called before the first frame update
    void Start()
    {
        Panel = GameObject.Find("Notice");
        Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Panel.SetActive(Microphone.devices.Length > 0);
    }
}