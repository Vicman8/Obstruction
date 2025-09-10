using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorCube;       // assign child cube
    public float openHeight = 8f;
    public float openSpeed = 2f;
    public float openDuration = 3f;

    [Header("Indicator Light")]
    public Renderer lightIndicator;  // sphere MeshRenderer for emission
    public Light indicatorLight;     // child Point Light
    public Material redMat;
    public Material greenMat;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpening = false;

    void Start()
    {
        // Store initial position
        closedPos = doorCube.localPosition;
        openPos = closedPos + Vector3.up * openHeight;

        // Set light and material to red initially
        if (lightIndicator != null && redMat != null)
            lightIndicator.material = redMat;

        if (indicatorLight != null)
            indicatorLight.color = Color.red;
    }

    public void OpenDoor()
    {
        if (!isOpening)
            StartCoroutine(OpenAndClose());
    }

    private IEnumerator OpenAndClose()
    {
        isOpening = true;

        // Set light and material to green
        if (lightIndicator != null && greenMat != null)
            lightIndicator.material = greenMat;

        if (indicatorLight != null)
            indicatorLight.color = Color.green;

        // Animate door moving up
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorCube.localPosition = Vector3.Lerp(closedPos, openPos, t);
            yield return null;
        }

        // Wait while door stays open
        yield return new WaitForSeconds(openDuration);

        // Animate door moving back down
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorCube.localPosition = Vector3.Lerp(openPos, closedPos, t);
            yield return null;
        }

        // Reset light and material to red
        if (lightIndicator != null && redMat != null)
            lightIndicator.material = redMat;

        if (indicatorLight != null)
            indicatorLight.color = Color.red;

        isOpening = false;
    }
}