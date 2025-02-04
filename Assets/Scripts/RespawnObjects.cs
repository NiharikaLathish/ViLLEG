using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToRespawn;
    private Vector3[] respawnPoints;
    private Quaternion[] respawnRotations;

    void Awake()
    {
        respawnPoints = new Vector3[objectsToRespawn.Length];
        respawnRotations = new Quaternion[objectsToRespawn.Length];
        for (int i = 0; i < objectsToRespawn.Length; i++)
        {
            respawnPoints[i] = objectsToRespawn[i].transform.position;
            respawnRotations[i] = objectsToRespawn[i].transform.rotation;
        }
    }

    void Update()
    {
        for (int i = 0; i < objectsToRespawn.Length; i++)
        {
            if (objectsToRespawn[i].transform.position.y < 0.04)
            {
                Rigidbody rb = objectsToRespawn[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                objectsToRespawn[i].transform.position = respawnPoints[i];
                objectsToRespawn[i].transform.rotation = respawnRotations[i];

            }
        }
    }
}