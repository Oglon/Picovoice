using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneVisual : MonoBehaviour
{
    [field: SerializeField] public Sprite Active { get; set; }
    [field: SerializeField] public Sprite Inactive { get; set; }
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.sprite = Inactive;
    }

    public void IsActive()
    {
        image.sprite = Active;
    }

    public void IsInactive()
    {
        image.sprite = Inactive;
    }
}