using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CameraFollow : MonoBehaviour 
{
    private Renderer[] playerRenderers;
    public Transform player;
    public Transform firstPersonPoint;

    [Header("Third Person")]
    public float distance = 5f;
    public float height = 1.6f;

    [Header("First Person")]
    public float firstPersonHeight = 1.65f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 3f;
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 65f;

    [Header("Camera Collision")]
    public LayerMask obstacleLayers;
    public float collisionOffset = 0.2f;

    private float rotationX = 15f;
    private float rotationY = 0f;

    public float cameraCollisionRadius = 0.15f;

    // false = 第三人称，true = 第一人称
    private bool isFirstPerson = false;
    public bool IsFirstPerson
    {
        get { return isFirstPerson; }
    }
    void UpdatePlayerVisibility()
    {
        foreach (Renderer renderer in playerRenderers)
        {
            if (isFirstPerson)
            {
                renderer.shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            else
            {
                renderer.shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player is missing!");
            enabled = false;
            return;
        }

        rotationY = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerRenderers = player.GetComponentsInChildren<Renderer>();
    }

    void LateUpdate()
    {
        // 按 V 切换视角
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            UpdatePlayerVisibility();
        }

        // ESC 释放鼠标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 左键重新锁定鼠标
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // 只有锁定鼠标时才旋转
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            rotationY += mouseX * mouseSensitivity;
            rotationX -= mouseY * mouseSensitivity;
        }

        rotationX = Mathf.Clamp(
            rotationX,
            minVerticalAngle,
            maxVerticalAngle
        );

        Quaternion rotation =
            Quaternion.Euler(rotationX, rotationY, 0f);

        Vector3 targetPosition;

        if (isFirstPerson)
        {
            Vector3 desiredPosition = firstPersonPoint.position;

            Vector3 start = player.position + Vector3.up * 1.2f;

            Vector3 direction = desiredPosition - start;

            float distance = direction.magnitude;

            if (distance > 0.001f &&
                Physics.SphereCast(
                    start,
                    cameraCollisionRadius,
                    direction.normalized,
                    out RaycastHit hit,
                    distance,
                    obstacleLayers,
                    QueryTriggerInteraction.Ignore))
            {
                desiredPosition =
                    start + direction.normalized *
                    Mathf.Max(0f, hit.distance - 0.05f);
            }

            transform.position = desiredPosition;
        }
        else
        {
            // 第三人称：摄像机位于人物后方
            targetPosition =
                player.position +
                Vector3.up * height;

            Vector3 direction = rotation * Vector3.back;

            Vector3 desiredPosition =
                targetPosition + direction * distance;

            // 避免摄像机穿墙
            if (Physics.Raycast(
                targetPosition,
                direction,
                out RaycastHit hit,
                distance,
                obstacleLayers,
                QueryTriggerInteraction.Ignore))
            {
                desiredPosition =
                    hit.point - direction * collisionOffset;
            }

            transform.position = desiredPosition;
        }

        // 两种模式都使用鼠标控制视角
        transform.rotation = rotation;
    }
}