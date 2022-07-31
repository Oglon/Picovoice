using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTalkingRange : MonoBehaviour
{
    public PicovoiceImplementation rhino;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("In Talking Range");
        }
    }
}