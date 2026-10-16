using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public int checkpointOrder = 1;

    public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        CheckpointManager checkpointManager =
            other.GetComponentInParent<CheckpointManager>();

        if (checkpointManager == null)
        {
            return;
        }

        if (respawnPoint == null)
        {
            Debug.LogWarning(
                "Respawn Point is missing on " + gameObject.name
            );

            return;
        }

        bool activated = checkpointManager.SetCheckpoint(
            respawnPoint,
            checkpointOrder
        );

        if (activated)
        {
            Debug.Log(
                "Activated Checkpoint: " + gameObject.name
            );
        }
    }
}
