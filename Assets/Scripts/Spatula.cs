using UnityEngine;

public class Spatula : MonoBehaviour
{
    public GameObject Dishobj;
    private GameObject saltPrefab;
    public Transform saltSpawnPoint;
    public float saltAmount = 5f;
    private bool hasSalt = false;
    private Dish dishScript;
    private void Awake()
    {
        dishScript = Dishobj.GetComponent<Dish>();
        saltPrefab = this.transform.Find("salt").gameObject;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "SaltBottle" && !hasSalt)
        {
            PickupSalt();
        }
        else if (collision.gameObject.name == "Dish" && hasSalt)
        {
            print("collide");
            TransferSaltToDish(collision.gameObject);
        }
    }

    private void PickupSalt()
    {

        
        if(!hasSalt)
        {
            saltPrefab.SetActive(true);    
            hasSalt = true;

        }

    }

    private void TransferSaltToDish(GameObject dish)
    {
       
        print("collision detected");
        saltPrefab.SetActive(false);
        hasSalt = false;
        dishScript.AddSalt(saltAmount);
    }
}
