using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private UnityEvent _event;

    [SerializeField]
    private float countDown = 30.0f;
    float initial_value = 0;
    float time_elapsed = 0;

    void Start()
    {
        initial_value = countDown;
    }

    void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
        }
        time_elapsed = initial_value - countDown;
        if (countDown <= 0)
        {
            TimesUp();
        }
    }

    void TimesUp()
    {
        _event.Invoke();
    }
}
