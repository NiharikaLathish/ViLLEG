using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffButton : MonoBehaviour
{
    public WeighingBalance WeighingBalance;
    private bool onOff = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Left Hand" || other.gameObject.tag == "Right Hand")
        {
            onOff = !onOff;
            WeighingBalance.OnOffButton(onOff);
        }
    }
}
