using System.Collections;
using System.Collections.Generic;



using UnityEngine;
using TMPro;

public class SprintTutorialZone : MonoBehaviour
{
    public TMP_Text tutorialText;
    public PlayerMovement player;

    public float minSprintSpeed = 6.5f;

    private Rigidbody rb;

    private bool tutorialStarted = false;
    private bool completed = false;

    public bool IsCompleted()
    {
        return completed;
    }

    void Start()
    {
        if (player != null)
        {
            rb = player.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // 跳跃教学还没完成，不检测冲刺
        if (!tutorialStarted || completed)
        {
            return;
        }

        if (rb == null)
        {
            return;
        }

        bool holdingShift = Input.GetKey(KeyCode.LeftShift);

        Vector3 velocity = rb.velocity;
        velocity.y = 0f;

        float horizontalSpeed = velocity.magnitude;

        if (holdingShift && horizontalSpeed >= minSprintSpeed)
        {
            completed = true;

            if (tutorialText != null)
            {
                tutorialText.text = tutorialText.text = tutorialText.text = "Hold Shift + Space to Jump Farther!"; ;
                tutorialText.gameObject.SetActive(true);
            }

            Debug.Log("Sprint tutorial completed!");
        }
    }

    // 开始冲刺教学
    public void BeginSprintTutorial()
    {
        if (tutorialStarted)
        {
            return;
        }

        tutorialStarted = true;

        if (tutorialText != null)
        {
            tutorialText.text = "Hold Shift to Sprint";
            tutorialText.gameObject.SetActive(true);
        }
    }
}

