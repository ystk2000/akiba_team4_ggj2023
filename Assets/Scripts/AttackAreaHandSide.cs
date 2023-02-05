using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaHandSide : MonoBehaviour
{
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 2f)
        {
            Destroy(this.gameObject);
        }        
    }
}
