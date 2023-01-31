using UnityEngine;

public class Phone : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioClip ring;

    private Transform player;

    [field: SerializeField] public Objective Objective;
    [field: SerializeField] private ObjectiveHandler objectiveHandler;

    private float timer = 3f;

    private float UseRange = 4;

    private Quest currentQuest;
    private Objective currentObjective;

    private bool active;

    private void Start()
    {
        player = GameObject.Find("PlayerCapsule").gameObject.transform;

        currentQuest = objectiveHandler.GetCurrentQuest();

        ring = Resources.Load<AudioClip>("Ring");
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = ring;
    }

    private void Update()
    {
        currentQuest = objectiveHandler.GetCurrentQuest();
        currentObjective = currentQuest.currentObjective;
        if (Objective == currentObjective && active != true)
        {
            active = true;
        }

        if (active)
        {
            Ready();
        }

        if (active && Objective != currentObjective)
        {
            Deactivate();
        }
    }

    private void Ready()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            _audioSource.Play();
            timer = 3f;
        }


        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= UseRange && Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }
    }

    private void Use()
    {
        currentQuest.Progress();
        Deactivate();
    }

    public void Deactivate()
    {
        active = false;
    }
}