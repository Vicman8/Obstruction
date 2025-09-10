using TMPro;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public TextMeshProUGUI promptText;   // assign your UI text
    public float interactDistance = 3f;  // how close player needs to be
    public Rigidbody doorRigidbody;      // Rigidbody of the door
    public float kickForce = 500f;       // how strong the kick is

    private Transform player;
    private bool playerInRange = false;

    void Start()
    {
        promptText.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (doorRigidbody != null)
            doorRigidbody.isKinematic = true; // keep it still until kicked
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
        {
            if (!playerInRange)
            {
                promptText.text = "Press E to Kick the Door!";
                promptText.gameObject.SetActive(true);
                playerInRange = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                KickDoor();
            }
        }
        else
        {
            if (playerInRange)
            {
                promptText.gameObject.SetActive(false);
                playerInRange = false;
            }
        }
    }

    void KickDoor()
    {
        if (doorRigidbody != null)
        {
            doorRigidbody.isKinematic = false;                  // let physics take over
            doorRigidbody.AddForce(transform.forward * kickForce); // apply forward force
            doorRigidbody.AddTorque(Vector3.up * 200f);        // optional spin
        }

        promptText.gameObject.SetActive(false);
    }
}
