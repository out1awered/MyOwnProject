using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float rotationSpeed = 90f;

    // 门打开的旋转角度
    public float openAngle = 90f;

    private bool isOpen = false;

    public bool isLocked = true;

    private Quaternion closedRotation;

    void Start()
    {
        // 记录门关闭时的初始角度
        closedRotation = transform.localRotation;
    }

    void Update()
    {
        // 根据 Inspector 中的角度计算开门方向
        Quaternion openRotation = closedRotation *
            Quaternion.Euler(0, openAngle, 0);

        Quaternion targetRotation;

        if (isOpen)
        {
            targetRotation = openRotation;
        }
        else
        {
            targetRotation = closedRotation;
        }

        transform.localRotation =
            Quaternion.RotateTowards(
                transform.localRotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }

    public void ToggleDoor()
    {
        if (isLocked)
        {
            return;
        }

        isOpen = !isOpen;
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }
}

