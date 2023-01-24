using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; set; }
    [field: SerializeField] public ObjectiveHandler ObjectiveHandler { get; set; }

    [field: SerializeField] public AudioBar Slider { get; set; }
    [field: SerializeField] public MicrophoneVisual MicrophoneVisual { get; set; }

    [field: SerializeField] public AudioSource WalkingSource { get; set; }
    [field: SerializeField] public AudioSource AmbientSource { get; set; }

    [field: SerializeField] public AudioClip AmbientClip { get; set; }

    [field: SerializeField] public AudioClip Walking1 { get; set; }
    [field: SerializeField] public AudioClip Walking2 { get; set; }
    [field: SerializeField] public AudioClip Walking3 { get; set; }
    

    private Vector3 _oldPos;
    private float _distanceTravelled;
    private Vector3 _distanceVector;

    private void Start()
    {
        PlayOfficeAmbience();
        _oldPos = transform.position;
    }

    private void Update()
    {
        Vector3 distanceVector = transform.position - _oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        _distanceTravelled += distanceThisFrame;
        _oldPos = transform.position;

        if (_distanceTravelled >= 2)
        {
            WalkingSound();
        }
    }

    private void PlayOfficeAmbience()
    {
        AmbientSource.clip = AmbientClip;
        AmbientSource.Play();
    }

    private void WalkingSound()
    {
        _distanceTravelled = 0;

        System.Random rnd = new System.Random();
        int responseInt = rnd.Next(1, 4);

        switch (responseInt)
        {
            case 1:
                PlayWalkingSound(Walking1);
                break;

            case 2:
                PlayWalkingSound(Walking2);
                break;

            case 3:
                PlayWalkingSound(Walking3);
                break;
        }
    }

    private void PlayWalkingSound(AudioClip clip)
    {
        WalkingSource.clip = clip;
        WalkingSource.Play();
    }
}