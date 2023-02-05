using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] AudioSource damageSound;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�U�����󂯂܂���");
        if (other.tag == "BossAttack")
        {
            damageSound.Play();
            GManager.instance.ReducePlayerHP(3);
        }
    }
}
