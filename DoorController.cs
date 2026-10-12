using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float rotationSpeed = 90f;

    private bool isOpen = false;

    public bool IsOpen()
    {
        return isOpen;
    }

    public bool isLocked = true;



    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;

        openRotation = closedRotation *
            Quaternion.Euler(0, 90, 0);
    }

    void Update()
    {
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

    public bool IsLocked()
    {
        return isLocked;
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }
}
