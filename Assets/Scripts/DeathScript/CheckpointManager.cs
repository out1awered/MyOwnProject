using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Initial Checkpoint")]
    public Transform initialCheckpoint;

    public Transform CurrentCheckpoint { get; private set; }

    // 当前已经到达的最高检查点编号
    public int CurrentCheckpointOrder { get; private set; }

    void Awake()
    {
        CurrentCheckpoint = initialCheckpoint;
        CurrentCheckpointOrder = 0;

        if (initialCheckpoint == null)
        {
            Debug.LogError("Initial Checkpoint is missing!");
        }
    }

    public bool SetCheckpoint(Transform newCheckpoint, int newOrder)
    {
        if (newCheckpoint == null)
        {
            return false;
        }

        // 禁止检查点倒退或重复激活
        if (newOrder <= CurrentCheckpointOrder)
        {
            Debug.Log(
                "Checkpoint ignored: " + newOrder +
                " | Current: " + CurrentCheckpointOrder
            );

            return false;
        }

        // 更新检查点
        CurrentCheckpoint = newCheckpoint;
        CurrentCheckpointOrder = newOrder;

        Debug.Log(
            "Checkpoint Updated! Order: " +
            CurrentCheckpointOrder
        );

        return true;
    }
}
