using UnityEngine;
using TMPro;

public class WeighingBalance : MonoBehaviour
{
    private Vector3 snapPosition;
    public GameObject weightDisplay;
    private float currentWeight = 0f; // Total weight including dish and salt
    private float dishWeight = 25f;   // Dish weight
    private bool isBalanceOn = false;
    private bool tared = false;
    private bool showingSaltOnly = false; // Toggle between showing only salt or including dish weight
    private float tareOffset = 0f;    // Offset used for taring, stored in grams
    private string currentUnit = "g"; // Default unit is grams
    private TextMeshProUGUI textComponent;
    public Dish dishScript;

    private void Start()
    {
        snapPosition = new Vector3(0.139799997f, 0.647000015f, -3.68799996f);
        textComponent = weightDisplay.GetComponent<TextMeshProUGUI>();
        weightDisplay.SetActive(false);
    }

    private void Update()
    {
        if (isBalanceOn)
        {
            UpdateWeightDisplay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleDishPlacement(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleDishPlacement(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (other.gameObject.tag == "Dish")
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            // Clear weight and tare state if dish is removed
            if (!tared)
            {
                currentWeight = 0f;
            }
            else
            {
                // If tared, just reset the salt weight as tare is still valid
                currentWeight = dishScript.GetSaltWeight();
            }
        }
    }

    private void HandleDishPlacement(Collider other)
    {
        if (other.gameObject.tag == "Dish")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            currentWeight = dishScript.GetSaltWeight() + dishWeight; // Always calculate weight as (salt + dish)
            rb.useGravity = false;
            rb.isKinematic = true;
            other.gameObject.transform.position = snapPosition;
            other.gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    public void TareScale()
    {
        if (isBalanceOn)
        {
            if (!tared)
            {
                // Set tare offset to the current total weight
                tareOffset = currentWeight; // Sets tare for (dish + salt) combined
                showingSaltOnly = true;
                tared = true;
            }
            else
            {
                // Reset tare and toggle showingSaltOnly
                if (showingSaltOnly)
                {
                    tareOffset = 0f; // Reset tare offset
                    showingSaltOnly = false;
                }
                else
                {
                    tareOffset = currentWeight; // Tare again to show salt only
                    showingSaltOnly = true;
                }
            }
            UpdateWeightDisplay();
        }
    }

    public void ChangeUnit()
    {
        // Reset to grams before switching units to avoid compounding errors
        float weightInGrams = ConvertToGrams(currentWeight);

        // Cycle through units
        switch (currentUnit)
        {
            case "g":
                currentUnit = "kg";
                weightInGrams /= 1000f; // Convert to kilograms
                break;
            case "kg":
                currentUnit = "mg";
                weightInGrams *= 1000000f; // Convert to milligrams
                break;
            case "mg":
                currentUnit = "g";
                weightInGrams = weightInGrams / 1000f / 1000f; // Reset to grams
                break;
        }

        currentWeight = weightInGrams;
        UpdateWeightDisplay();
    }

    private float ConvertToGrams(float weight)
    {
        switch (currentUnit)
        {
            case "kg":
                return weight * 1000f;
            case "mg":
                return weight / 1000f;
            case "g":
            default:
                return weight;
        }
    }

    public void UpdateWeightDisplay()
    {
        float displayedWeight;

        if (showingSaltOnly)
        {
            // Show weight of the salt only
            displayedWeight = Mathf.Max(currentWeight - dishWeight - tareOffset, 0);
        }
        else
        {
            // Show weight including the dish weight
            displayedWeight = Mathf.Max(currentWeight - tareOffset, 0);
        }

        // Convert weight for display based on the current unit
        switch (currentUnit)
        {
            case "kg":
                displayedWeight /= 1000f; // Convert grams to kilograms
                break;
            case "mg":
                displayedWeight *= 1000f; // Convert grams to milligrams
                break;
            case "g":
            default:
                break; // Already in grams
        }

        textComponent.text = displayedWeight.ToString("F2") + " " + currentUnit;
    }

    public void OnOffButton(bool b)
    {
        weightDisplay.SetActive(b);
        isBalanceOn = b;

        if (isBalanceOn)
        {
            currentWeight = dishWeight; // Initialize with dish weight
            UpdateWeightDisplay();
        }
    }
}
