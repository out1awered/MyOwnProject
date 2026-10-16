using System.Collections;
using System.Collections.Generic;


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RespawnController : MonoBehaviour
{
    [Header("References")]
    public CheckpointManager checkpointManager;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Respawn Settings")]
    public float respawnHeight = 8f;
    public float groundCheckRadius = 0.2f;
    public float maxLandingWait = 12f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (checkpointManager == null)
        {
            checkpointManager = GetComponent<CheckpointManager>();
        }
    }

    public IEnumerator Respawn(RespawnMode mode)
    {
        if (checkpointManager == null ||
            checkpointManager.CurrentCheckpoint == null)
        {
            Debug.LogError("Checkpoint is not assigned!");
            yield break;
        }

        Transform checkpoint =
            checkpointManager.CurrentCheckpoint;

        // 清除死亡前的速度
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 targetPosition = checkpoint.position;

        if (mode == RespawnMode.FallFromSky)
        {
            targetPosition += Vector3.up * respawnHeight;
        }

        // 移动到复活位置
        rb.position = targetPosition;
        rb.rotation = checkpoint.rotation;

        Physics.SyncTransforms();

        // 直接复活模式
        if (mode == RespawnMode.AtCheckpoint)
        {
            yield return new WaitForFixedUpdate();
            yield break;
        }

        // 从天空复活，等待落地
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck is missing!");
            yield break;
        }

        float elapsed = 0f;

        yield return new WaitForFixedUpdate();

        while (!Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore))
        {
            if (elapsed >= maxLandingWait)
            {
                Debug.LogWarning(
                    "Respawn landing timeout! Check Ground Layer."
                );
                yield break;
            }

            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;
        }

        Debug.Log("Player landed after respawn!");
    }
}
