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

    public static int collection = 0;

    [field: SerializeField] public AudioClip PickUpSound { get; private set; }
    private AudioSource audioSource;

    private float pickUpRange = 4;

    private void Start()
    {
        player = GameObject.Find("PlayerCapsule").gameObject.transform;
        container = GameObject.Find("Container").gameObject.transform;

        player.TryGetComponent<AudioSource>(out audioSource);
        coll = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        objectiveHandler = GameObject.Find("PlayerCapsule").GetComponent<ObjectiveHandler>();
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
        Debug.Log("PickUp");
        if (gameObject.CompareTag("QuestItem"))
        {
            currentQuest.Progress();
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("CollectionItem"))
        {
            collection++;
            if (collection == 3)
            {
                currentQuest.Progress();
            }

            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Goal"))
        {
            SceneManager.LoadScene("StartMenu");
        }

        _rigidbody.isKinematic = true;
        equipped = true;
        SlotFull = true;

        transform.parent = container;
        transform.position = container.position;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        coll.isTrigger = true;

        audioSource.clip = PickUpSound;
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

    // public void Delete()
    // {
    //     Drop();
    //     Destroy(gameObject);
    // }
}