using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseTarget : MonoBehaviour
{
    public Transform targetTransform;

    public bool lookat = false;
    Vector3 computer = new Vector3(0.7f, 1.2f, -0.16f);

    // Update is called once per frame
    void Update()
    {
        if (lookat)
        {
            transform.position = targetTransform.position;
        }
        else
        {
            transform.localPosition = computer;
        }
    }
}