using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVertical : MonoBehaviour
{
    public Quaternion firstRotation;
	public float speedY = 100;
    private float elapsedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        firstRotation = transform.rotation;
        elapsedTime = 0;

		Rigidbody rigidbody = GetComponent<Rigidbody>();
 
		Vector3 movementSpeed = new Vector3 (0, speedY, 0);
 
		movementSpeed = firstRotation * movementSpeed;
 
		rigidbody.AddForce(movementSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 1)
        {
            Destroy(this.gameObject);
        }   
    }
}
