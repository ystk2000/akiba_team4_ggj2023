using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSide : MonoBehaviour
{
    public Quaternion firstRotation;
	public float speedX = 100;
    private float elapsedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        firstRotation = transform.rotation;
        elapsedTime = 0;

		Rigidbody rigidbody = GetComponent<Rigidbody>();
 
		Vector3 movementSpeed = new Vector3 (speedX, 0, 0);
 
		movementSpeed = firstRotation * movementSpeed;
 
		rigidbody.AddForce(movementSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 10f)
        {
            Destroy(this.gameObject);
        }   
    }
}
