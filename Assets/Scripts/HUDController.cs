using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Ability Status Texts")]
    public TextMeshProUGUI walkText;
    public TextMeshProUGUI sprintText;
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI lookText;
    public TextMeshProUGUI buttonText;

    [Header("Battery Display")]
    public TextMeshProUGUI batteryText;
    private int batteryPercent = 100;

    // Singleton style
    public static HUDController Instance;

    void Awake()
    {
        Instance = this;
        UpdateBatteryDisplay();
    }

    // Only updates the ability text color
    public void DisableAbility(string abilityName)
    {
        switch (abilityName)
        {
            case "Walk":
                walkText.color = Color.red;
                break;
            case "Sprint":
                sprintText.color = Color.red;
                break;
            case "Jump":
                jumpText.color = Color.red;
                break;
            case "Look":
                lookText.color = Color.red;
                break;
            case "PressButtons":
                buttonText.color = Color.red;
                break;
        }
    }

    // Separate method to decrease battery
    public void DecreaseBattery(int amount)
    {
        batteryPercent -= amount;
        if (batteryPercent < 0) batteryPercent = 0;
        UpdateBatteryDisplay();
    }

    private void UpdateBatteryDisplay()
    {
        batteryText.text = "Battery: " + batteryPercent + "%";
    }
}