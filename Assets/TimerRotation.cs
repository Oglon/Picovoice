using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerRotation : MonoBehaviour
{
    [field: SerializeField] public GameObject Player { get; private set; }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
    }
}