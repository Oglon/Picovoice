using UnityEngine;

public class KeycardReader : MonoBehaviour
{
    private Transform player;

    [field: SerializeField] public Objective Objective;
    [field: SerializeField] private ObjectiveHandler objectiveHandler;

    [field: SerializeField] private Animator myDoor;

    private float UseRange = 4;

    private Quest currentQuest;
    private Objective currentObjective;

    private bool active = false;

    private Outline _outline;


    private void Start()
    {
        player = GameObject.Find("PlayerCapsule").gameObject.transform;

        currentQuest = objectiveHandler.GetCurrentQuest();
        _outline = gameObject.GetComponent<Outline>();
    }

    private void Update()
    {
        currentQuest = objectiveHandler.GetCurrentQuest();
        currentObjective = currentQuest.currentObjective;
        if (Objective == currentObjective && active != true)
        {
            Activate();
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

    public void Activate()
    {
        _outline.OutlineMode = Outline.Mode.OutlineAll;
    }

    private void Ready()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= UseRange && Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }
    }

    private void Use()
    {
        myDoor.Play("DoorOpening", 0, 0.0f);
        currentQuest.Progress();
        Deactivate();
    }

    public void Deactivate()
    {
        active = false;
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
    }
}