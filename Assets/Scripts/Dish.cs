using UnityEngine;

public class Dish : MonoBehaviour
{
    public Transform saltSpawnPoint;
    private GameObject saltPrefab;
    private float totalSaltWeight = 0f;
    private Vector3 baseScale;

    private void Awake()
    {
        saltPrefab = this.transform.Find("salt").gameObject;
        baseScale = saltPrefab.transform.localScale; // Store the initial scale of the salt
    }

    public void AddSalt(float saltAmount)
    {
        totalSaltWeight += saltAmount;
        InstantiateSaltVisual(totalSaltWeight);
    }

    private void InstantiateSaltVisual(float saltAmount)
    {
        saltPrefab.SetActive(true);

        // Reset to the original base scale before applying the new scale
        saltPrefab.transform.localScale = baseScale;

        // Calculate a scale factor based on the salt weight
        float scaleFactor = CalculateScaleFactor(saltAmount);

        // Apply the scale factor uniformly
        saltPrefab.transform.localScale = baseScale * scaleFactor;
    }

    // Function to calculate the scale factor based on the salt weight
    private float CalculateScaleFactor(float saltAmount)
    {
        // Define the scaling rules for specific weights
        if (saltAmount <= 0.1f)
        {
            return 0.5f; // A smaller scale factor for 0.1g
        }
        else if (saltAmount <= 1f)
        {
            return 1f; // Base scale factor for 1g
        }
        else if (saltAmount <= 5f)
        {
            return 2f; // Larger scale for 5g
        }
        else
        {
            return 3f; // Further scale for amounts greater than 5g
        }
    }

    public float GetSaltWeight()
    {
        return totalSaltWeight;
    }

    public void dropObject()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
