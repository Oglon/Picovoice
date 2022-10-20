using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private BoxCollider coll;
    private Transform player, container;

    public bool equipped;
    public static bool slotFull;

    private float pickUpRange = 4;

    private void Start()
    {
        player = GameObject.Find("PlayerCapsule").gameObject.transform;
        container = GameObject.Find("Container").gameObject.transform;

        coll = GetComponentInChildren<BoxCollider>();
    }

    private void Update()
    {
        if (!gameObject.name.Contains("Cesar"))
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (!equipped && distanceToPlayer <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
            PickUp();

        if (equipped && Input.GetKeyDown(KeyCode.Q))
            Drop();
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        // transform.localScale = Vector3.one;

        coll.isTrigger = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        coll.isTrigger = false;
    }

    public void Delete()
    {
        Drop();
    }
}