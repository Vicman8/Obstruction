using UnityEngine;
using TMPro;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    public LaserWall laserWall; // assign your laser wall object
    public TMP_Text warningText;    // assign UI text
    public float warningDuration = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Only start the laser if it’s not moving
        if (!laserWall.IsMoving())
        {
            // Show warning text
            if (warningText) warningText.text = "GET TO THE DOOR FAST!";
            if (warningText) StartCoroutine(HideWarningText());

            // Start laser
            if (laserWall) laserWall.StartMoving();
        }
    }

    private IEnumerator HideWarningText()
    {
        yield return new WaitForSeconds(warningDuration);
        warningText.text = "";
    }
}
