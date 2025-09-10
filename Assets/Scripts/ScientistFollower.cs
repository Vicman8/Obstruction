using UnityEngine;

public class ScientistFollower : MonoBehaviour
{
    public Transform player;         // Assign the player
    public Vector3 hallwayDirection = Vector3.forward; // Hallway axis
    public float moveSpeed = 3f;
    public float rotateSpeed = 5f;

    private Vector3 lastPlayerPos;

    void Start()
    {
        lastPlayerPos = player.position;
    }

    void Update()
    {
        // Project player movement onto hallway direction
        Vector3 delta = player.position - lastPlayerPos;
        float movementAlongHallway = Vector3.Dot(delta, hallwayDirection.normalized);

        if (Mathf.Abs(movementAlongHallway) > 0.001f)
        {
            // Move scientist along hallway
            transform.position += hallwayDirection.normalized * movementAlongHallway;

            // Rotate to face hallway movement
            Quaternion moveRot = Quaternion.LookRotation(hallwayDirection.normalized * Mathf.Sign(movementAlongHallway), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRot, Time.deltaTime * rotateSpeed);
        }
        else
        {
            // Player stopped along hallway → face the player
            Vector3 lookDir = (player.position - transform.position);
            lookDir.y = 0; // keep upright
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
            }
        }

        lastPlayerPos = player.position;
    }
}
