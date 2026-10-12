using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float rayDistance = 10f;
    public float pickupDistance = 3f;
    public float detectionRadius = 3f;

    [Header("UI")]
    public TMP_Text interactionText;

    [Header("Camera")]
    public CameraFollow cameraFollow;

    [Header("Obstacle Detection")]
    public LayerMask obstacleLayer;

    bool IsBlocked(Interactable item)
    {
        // 从玩家身体附近开始检测
        Vector3 start = transform.position + Vector3.up;

        // 找到目标碰撞体
        Collider targetCollider =
            item.GetComponentInChildren<Collider>();

        if (targetCollider == null)
            return true;

        // 找到目标上离检测起点最近的位置
        Vector3 end = targetCollider.ClosestPoint(start);

        Vector3 direction = end - start;

        float distance = direction.magnitude;

        if (distance < 0.01f)
            return false;

        // 检测路线上是否有障碍物
        return Physics.Raycast(
            start,
            direction.normalized,
            distance,
            obstacleLayer,
            QueryTriggerInteraction.Ignore
        );
       
}

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Interactable target = null;

        if (cameraFollow != null && cameraFollow.IsFirstPerson)
        {
            target = DetectFirstPerson();
        }
        else
        {
            target = DetectThirdPerson();
        }

        if (interactionText != null)
        {
            if (target != null)
            {
                interactionText.text = target.GetInteractionText();
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }

        if (target != null && Input.GetKeyDown(KeyCode.E))
        {
            target.Interact();

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    Interactable DetectFirstPerson()
    {
        Camera cam = Camera.main;

        if (cam == null)
            return null;

        Ray ray = new Ray(
            cam.transform.position,
            cam.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            rayDistance,
            ~0,
            QueryTriggerInteraction.Collide))
        {
            Interactable item =
                hit.collider.GetComponentInParent<Interactable>();

            if (item != null)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    hit.collider.ClosestPoint(transform.position)
                );

                if (distance <= pickupDistance)
                    return item;
            }
        }

        return null;
    }

    Interactable DetectThirdPerson()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            detectionRadius
        );

        Interactable nearestItem = null;
        float nearestDistance = float.MaxValue;

        Vector3 forward = Camera.main != null
            ? Camera.main.transform.forward
            : transform.forward;

        forward.y = 0f;
        forward.Normalize();

        foreach (Collider col in colliders)
        {
            Interactable item =
                col.GetComponentInParent<Interactable>();

            if (item == null)
                continue;

            if (IsBlocked(item))
                continue;

            Vector3 direction =
                item.transform.position - transform.position;

            direction.y = 0f;

            if (Vector3.Dot(forward, direction.normalized) < 0.3f)
                continue;

            float distance = direction.magnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestItem = item;
            }
        }

        return nearestItem;
    }
}