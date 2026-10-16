using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AbyssDeathDetector : MonoBehaviour
{
    public PlayerDeathManager deathManager;

    [Header("Abyss Settings")]
    public float deathHeight = -15f;

    void Awake()
    {
        if (deathManager == null)
        {
            deathManager = GetComponent<PlayerDeathManager>();
        }
    }

    void FixedUpdate()
    {
        if (deathManager == null || deathManager.IsDead())
        {
            return;
        }

        if (transform.position.y < deathHeight)
        {
            deathManager.Die(DeathType.Abyss);
        }
    }
}
