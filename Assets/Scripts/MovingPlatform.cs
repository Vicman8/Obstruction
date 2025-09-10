using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Vector3 target;
    private Vector3 lastPosition;

    [HideInInspector]
    public Vector3 platformVelocity; // velocity for player to use

    void Start()
    {
        target = pointB.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Move platform
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Calculate velocity
        platformVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        // Switch target
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }
    }
}
