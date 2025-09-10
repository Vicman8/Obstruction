using UnityEngine;

public class LaserWall : MonoBehaviour
{
    public Transform pointA;        // start position
    public Transform pointB;        // end position
    public float moveSpeed = 2f;    // laser speed

    private Vector3 targetPos;
    private bool moving = false;

    void Start()
    {
        // Ensure laser is initially inactive
        gameObject.SetActive(false);
    }

    public bool IsMoving()
    {
        return moving;
    }

    void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Stop at PointB
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            moving = false;
        }
    }

    // Call this to start the laser
    public void StartMoving()
    {
        transform.position = pointA.position;
        targetPos = pointB.position;
        moving = true;
        gameObject.SetActive(true); // activate the laser
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Respawn the player
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.Respawn();
            }

            // Reset and deactivate laser
            ResetLaser();
        }
    }

    private void ResetLaser()
    {
        moving = false;
        transform.position = pointA.position;
        gameObject.SetActive(false); // deactivate the laser
    }
}