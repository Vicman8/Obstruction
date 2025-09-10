using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f; // how far the player can reach

    void Update()
    {
        // Left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // Check if the object has a ButtonPress script
                ButtonPress button = hit.collider.GetComponent<ButtonPress>();
                if (button != null)
                {
                    button.Press(); // call the press function
                }
            }
        }
    }
}
