using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpController : MonoBehaviour
{
    private BoxCollider coll;
    private Transform player, container;
    private Rigidbody _rigidbody;

    public bool equipped;
    public static bool SlotFull;

    private ObjectiveHandler objectiveHandler;
    private Quest currentQuest;

    [field: SerializeField] public AudioClip PickUpSound { get; private set; }
    private AudioSource audioSource;

    private float pickUpRange = 4;

    private Picovoice Picovoice;

    private void Start()
    {
        player = GameObject.Find("PlayerCapsule").gameObject.transform;
        container = GameObject.Find("Container").gameObject.transform;

        Picovoice = GameObject.Find("Picovoice").GetComponent<Picovoice>();

        PickUpSound = Resources.Load<AudioClip>("PickUp");
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        audioSource.clip = PickUpSound;

        coll = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        GameObject gameObject = GameObject.Find("NestedParent_Unpack");
        objectiveHandler = gameObject.transform.GetChild(2).gameObject.GetComponent<ObjectiveHandler>();

        // objectiveHandler = GameObject.Find("PlayerCapsule").GetComponent<ObjectiveHandler>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (!equipped && distanceToPlayer <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !SlotFull)
        {
            PickUp();
        }
        else
        {
            if (equipped && Input.GetKeyDown(KeyCode.E))
                Drop();
        }
    }

    private void PickUp()
    {
        currentQuest = objectiveHandler.GetCurrentQuest();

        if (gameObject.CompareTag("QuestItem"))
        {
            audioSource.Play();
            currentQuest.Progress();
            Destroy(gameObject);
            return;
        }

        if (gameObject.CompareTag("CollectionItem"))
        {
            audioSource.Play();
            currentQuest.Progress();

            Destroy(transform.gameObject);
            return;
        }

        if (gameObject.CompareTag("Goal"))
        {
            Picovoice.Delete();
            audioSource.Play();
            SceneManager.LoadScene("StartMenu");
            return;
        }

        _rigidbody.isKinematic = true;
        equipped = true;
        SlotFull = true;

        transform.parent = container;
        transform.position = container.position;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        coll.isTrigger = true;

        audioSource.Play();
    }

    private void Drop()
    {
        Debug.Log("Drop");
        _rigidbody.isKinematic = false;
        equipped = false;
        SlotFull = false;

        transform.parent = null;

        coll.isTrigger = false;
    }

    public void Delete()
    {
        Drop();
        Destroy(gameObject);
    }
}