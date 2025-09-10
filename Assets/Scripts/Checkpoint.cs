using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public bool disableSprint = false;
    public bool disableJump = false;
    public bool disableLook = false;
    public bool disableClick = false;
    public bool disableWalk = false; // 👈 for final checkpoint stop
    public int batteryDecreaseAmount = 20;

    [Header("Dialogue Settings")]
    public bool playDialogue = false;
    public TMP_Text scientistTextUI;
    public TMP_Text hintTextUI;
    [TextArea] public string scientistMessage = "Now your ability to use your arms have been disabled, get creative to figure out how to move on through the door";
    [TextArea] public string hintMessage = "Get closer to the closed door";
    public float dialogueDuration = 3f;
    public float hintDelay = 1f;

    [Header("Final Checkpoint Settings")]
    public bool isFinalCheckpoint = false;   // toggle for the last checkpoint
    public TMP_Text statusTextUI;            // assign a TMP text for “Battery empty...”
    public Image fadeImage;                  // fullscreen black image (alpha=0 at start)
    public float fadeDuration = 3f;
    public float batteryEmptyDuration = 4f;  // how long the dot loop runs
    public float batteryEmptySpeed = 0.5f;   // speed of dot animation

    [Header("Restart Settings")]
    public Button restartButton;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
        PlayerWalk playerWalk = other.GetComponent<PlayerWalk>();

        // Update checkpoint
        if (respawn != null)
            respawn.SetCheckpoint(transform.position + Vector3.up * 1f);

        bool disabledSomething = false;

        // Disable abilities
        if (disableSprint && playerWalk != null)
        {
            playerWalk.DisableSprint();
            HUDController.Instance.DisableAbility("Sprint");
            disabledSomething = true;
        }

        if (disableJump && playerWalk != null)
        {
            playerWalk.DisableJump();
            HUDController.Instance.DisableAbility("Jump");
            disabledSomething = true;
        }

        if (disableLook && playerWalk != null)
        {
            playerWalk.DisableLook();
            HUDController.Instance.DisableAbility("Look");
            disabledSomething = true;
        }

        if (disableClick && playerWalk != null)
        {
            playerWalk.DisableButtonPress();
            HUDController.Instance.DisableAbility("PressButtons");
            disabledSomething = true;
        }

        if (disableWalk && playerWalk != null)
        {
            playerWalk.canWalk = false;
            HUDController.Instance.DisableAbility("Walk");
            disabledSomething = true;
        }

        // Decrease battery only if something was actually disabled
        if (disabledSomething)
        {
            HUDController.Instance.DecreaseBattery(batteryDecreaseAmount);
            Debug.Log("Battery decreased by " + batteryDecreaseAmount + "% at this checkpoint.");
        }

        // Handle dialogue + hint sequence
        if (playDialogue && scientistTextUI != null && hintTextUI != null)
        {
            StartCoroutine(ShowDialogueSequence());
        }

        // Handle final checkpoint sequence
        if (isFinalCheckpoint)
        {
            if (playerWalk != null)
            {
                playerWalk.canWalk = false;
                playerWalk.canLook = false;
                playerWalk.canSprint = false;
                playerWalk.canJump = false;
                playerWalk.canPressButtons = false;
            }
            StartCoroutine(FinalShutdownSequence());
        }
    }

    private IEnumerator ShowDialogueSequence()
    {
        // Show scientist message
        scientistTextUI.text = scientistMessage;
        scientistTextUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(dialogueDuration);

        // Hide scientist message
        scientistTextUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(hintDelay);

        // Show hint
        hintTextUI.text = hintMessage;
        hintTextUI.gameObject.SetActive(true);

        float hintDisplayDuration = 3f; // adjust how long the hint stays visible
        yield return new WaitForSeconds(hintDisplayDuration);

        // Hide hint
        hintTextUI.gameObject.SetActive(false);
    }

    private IEnumerator FinalShutdownSequence()
    {
        string baseText = "Battery empty";
        string[] dots = { ".", "..", "..." };

        float elapsed = 0f;
        int index = 0;

        // Step 1: loop battery empty text
        if (statusTextUI != null)
            statusTextUI.color = Color.yellow; // make "Battery empty" yellow

        while (elapsed < batteryEmptyDuration)
        {
            if (statusTextUI != null)
            {
                statusTextUI.text = baseText + dots[index];
                statusTextUI.gameObject.SetActive(true);
            }
            index = (index + 1) % dots.Length;

            yield return new WaitForSeconds(batteryEmptySpeed);
            elapsed += batteryEmptySpeed;
        }

        // Step 2: Shutting down message
        if (statusTextUI != null)
        {
            statusTextUI.color = Color.red; // change text color to red
            statusTextUI.text = "Shutting down";
        }

        yield return new WaitForSeconds(1f);

        // Step 3: Fade screen to black
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            float fadeElapsed = 0f;

            while (fadeElapsed < fadeDuration)
            {
                fadeElapsed += Time.deltaTime;
                color.a = Mathf.Clamp01(fadeElapsed / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
