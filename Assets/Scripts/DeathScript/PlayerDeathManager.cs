using System.Collections;
using System.Collections.Generic;


using UnityEngine;

[System.Serializable]
public class DeathRule
{
    public DeathType deathType;

    public float deathDelay = 1f;

    public RespawnMode respawnMode =
        RespawnMode.FallFromSky;
}

[RequireComponent(typeof(RespawnController))]
public class PlayerDeathManager : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public RespawnController respawnController;

    [Header("Death Rules")]
    public DeathRule[] deathRules;

    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (respawnController == null)
        {
            respawnController = GetComponent<RespawnController>();
        }
    }

    public void Die(DeathType type)
    {
        // 防止重复死亡
        if (isDead)
        {
            return;
        }

        if (respawnController == null)
        {
            Debug.LogError("RespawnController is missing!");
            return;
        }

        StartCoroutine(DeathRoutine(type));
    }

    IEnumerator DeathRoutine(DeathType type)
    {
        isDead = true;

        // 禁止玩家控制
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        float delay = 1f;

        RespawnMode mode = RespawnMode.FallFromSky;

        // 查找 Inspector 中的死亡规则
        if (deathRules != null)
        {
            foreach (DeathRule rule in deathRules)
            {
                if (rule != null && rule.deathType == type)
                {
                    delay = Mathf.Max(0f, rule.deathDelay);
                    mode = rule.respawnMode;
                    break;
                }
            }
        }

        Debug.Log("Player died: " + type);

        // 以后可以在这里调用死亡动画和音效
        yield return new WaitForSeconds(delay);

        // 执行复活
        yield return StartCoroutine(
            respawnController.Respawn(mode)
        );

        // 恢复移动
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        isDead = false;

        Debug.Log("Player respawn completed!");
    }
}
