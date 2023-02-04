using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float velocityZ;
    private float velocityY;
    private float timer;
    private const float lifetime = 3;
    private const float speedZ = 10;
    private const float speedY = 2f;
    private const float gravity = 0.1f;

    public void Init()
    {
        velocityZ = speedZ;
        velocityY = speedY;
        timer = lifetime;
    }

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        timer -= dt;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        transform.position += Vector3.forward * velocityZ * dt;
        transform.position += Vector3.up * velocityY * dt; 
        velocityY -= gravity;
    }


}
