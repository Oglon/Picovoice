using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    
    [field: SerializeField] public GameObject Player { get; private set; }
    
    // Update is called once per frame
    void Update()
    {
    gameObject.transform.LookAt(Player.transform);    
    }
}
