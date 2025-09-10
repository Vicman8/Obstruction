using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 currentCheckpoint;

    private void Start()
    {
        // Start position is first checkpoint by default
        currentCheckpoint = transform.position;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        // Save checkpoint slightly above its transform so player doesn’t sink
        currentCheckpoint = newCheckpoint + Vector3.up * 1f;  // 1 unit above
        Debug.Log("Checkpoint set to: " + currentCheckpoint);
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint;
    }
}