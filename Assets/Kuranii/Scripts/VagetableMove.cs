using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagetableMove : MonoBehaviour
{
    Transform vagetableTrasform;
    // Start is called before the first frame update
    void Start()
    {
        vagetableTrasform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) 
        vagetableTrasform.position += Vector3.right * 0.1f;
        else if (Input.GetKey(KeyCode.LeftArrow))
        vagetableTrasform.position += Vector3.left * 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("“G‚É‚Ô‚Â‚©‚è‚Ü‚µ‚½");
        if(collision.gameObject.tag == "enemy")
        {
            GManager.instance.ReduceEnemyHP(3);
        }
    }
}
