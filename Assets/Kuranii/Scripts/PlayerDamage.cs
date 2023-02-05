using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("UŒ‚‚ğó‚¯‚Ü‚µ‚½");
        if (other.tag == "BossAttack")
        {
            GManager.instance.ReducePlayerHP(3);
        }
    }
}
