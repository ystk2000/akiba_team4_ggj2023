using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform playerTrasform;
    // Start is called before the first frame update
    void Start()
    {
        playerTrasform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            playerTrasform.position += Vector3.right * 0.1f;
        else if (Input.GetKey(KeyCode.A))
            playerTrasform.position += Vector3.left * 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("çUåÇÇéÛÇØÇ‹ÇµÇΩ");
        if (collision.gameObject.tag == "enemy")
        {
            GManager.instance.ReducePlayerHP(3);
        }
    }
}
