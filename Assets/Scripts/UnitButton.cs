using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public WeighingBalance WeighingBalance;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Left Hand" || other.gameObject.tag == "Right Hand")
        {
            WeighingBalance.ChangeUnit();
        }
    }
}
