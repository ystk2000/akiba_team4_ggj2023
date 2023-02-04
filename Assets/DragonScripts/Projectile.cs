using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float velocity;
    private float timer;
    private const float lifetime = 3;
    private const float speed = 1;

    public void Init(bool movingRight)
    {
        velocity = movingRight ? speed : -speed;
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

        transform.position += Vector3.right * speed;
    }


}
