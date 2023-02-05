using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private BigBoss actor;
    private Vector3 defaultPosition;

    void Start()
    {
        defaultPosition = transform.position;
        actor = FindObjectOfType<BigBoss>();
    }

    void Update()
    {
        transform.position = actor.transform.position + defaultPosition;
    }
}
