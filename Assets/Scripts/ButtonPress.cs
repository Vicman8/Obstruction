using UnityEngine;
using System.Collections;

public class ButtonPress : MonoBehaviour
{
    [Header("References")]
    public DoorController door; // assign in Inspector

    [Header("Button Settings")]
    public float pressDepth = 0.1f; // how far button moves into the wall
    public float pressSpeed = 5f;   // speed of button press animation

    private Vector3 originalPosition;
    private bool isPressed = false;

    void Start()
    {
        originalPosition = transform.localPosition; // store initial local position
    }

    // Call this function when the player interacts (raycast + left click)
    public void Press()
    {
        if (!isPressed)
        {
            isPressed = true;

            // Trigger the door
            if (door != null)
            {
                door.OpenDoor();
            }

            // Start the button press animation
            StartCoroutine(AnimatePress());
        }
    }

    private IEnumerator AnimatePress()
    {
        // Move along local X (in/out of wall)
        Vector3 targetPos = originalPosition + transform.right * -pressDepth;

        // Animate button moving in
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * pressSpeed);
            yield return null;
        }

        // Short pause at bottom
        yield return new WaitForSeconds(0.2f);

        // Animate button returning to original position
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * pressSpeed);
            yield return null;
        }

        isPressed = false;
    }
}