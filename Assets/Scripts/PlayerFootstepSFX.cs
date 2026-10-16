using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerFootstepSFX : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip walkSound;
    public AudioClip runSound;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Footstep Settings")]
    public float walkInterval = 0.48f;
    public float runInterval = 0.28f;
    public float sprintSpeedThreshold = 6.5f;
    public float minMoveSpeed = 0.5f;

    private AudioSource audioSource;
    private Rigidbody rb;
    private PlayerDeathManager deathManager;

    private float footstepTimer = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        deathManager = GetComponent<PlayerDeathManager>();
    }

    void Update()
    {
        // 死亡和复活时停止触发脚步声
        if (deathManager != null && deathManager.IsDead())
        {
            footstepTimer = 0f;
            return;
        }

        if (groundCheck == null)
        {
            return;
        }

        // 1. 检测是否站在地面
        bool isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        // 2. 计算水平移动速度
        Vector3 horizontalVelocity = new Vector3(
            rb.velocity.x,
            0f,
            rb.velocity.z
        );

        float speed = horizontalVelocity.magnitude;

        // 3. 检测是否按着移动键
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool hasMoveInput =
            horizontal != 0f || vertical != 0f;

        // 4. 静止或在空中时停止脚步声
        if (!isGrounded || !hasMoveInput || speed < minMoveSpeed)
        {
            footstepTimer = 0f;
            return;
        }

        // 5. 根据速度选择脚步声
        bool isRunning = speed >= sprintSpeedThreshold;

        float interval = isRunning
            ? runInterval
            : walkInterval;

        AudioClip clip = isRunning
            ? runSound
            : walkSound;

        // 6. 按固定时间间隔播放
        footstepTimer += Time.deltaTime;

        if (footstepTimer >= interval)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }

            footstepTimer = 0f;
        }
    }
}
