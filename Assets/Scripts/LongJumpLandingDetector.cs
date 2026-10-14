using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class LongJumpLandingDetector : MonoBehaviour
{
    public TMP_Text tutorialText;
    public SprintTutorialZone sprintTutorial;
    public PlayerMovement player;

    public float minJumpSpeed = 6.5f;

    private Rigidbody rb;
    private Collider platformCollider;

    private bool validJump = false;
    private bool completed = false;

    void Start()
    {
        if (player != null)
        {
            rb = player.GetComponent<Rigidbody>();
        }

        platformCollider = GetComponent<Collider>();
    }

    void Update()
    {
        // 冲刺教学没有完成，不检测长跳
        if (sprintTutorial == null ||
            !sprintTutorial.IsCompleted() ||
            completed)
        {
            return;
        }

        if (rb == null)
        {
            return;
        }

        bool holdingShift = Input.GetKey(KeyCode.LeftShift);
        bool pressingSpace = Input.GetKeyDown(KeyCode.Space);

        Vector3 velocity = rb.velocity;
        velocity.y = 0f;

        float horizontalSpeed = velocity.magnitude;

        // 检测玩家是否以足够速度进行冲刺跳跃
        if (holdingShift &&
            pressingSpace &&
            horizontalSpeed >= minJumpSpeed)
        {
            validJump = true;

            Debug.Log("Valid running jump detected!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (completed || !validJump)
        {
            return;
        }

        if (sprintTutorial == null ||
            !sprintTutorial.IsCompleted() ||
            platformCollider == null)
        {
            return;
        }

        PlayerMovement landingPlayer =
            collision.gameObject.GetComponentInParent<PlayerMovement>();

        if (landingPlayer == null || landingPlayer != player)
        {
            return;
        }

        float platformTop = platformCollider.bounds.max.y;

        foreach (ContactPoint contact in collision.contacts)
        {
            bool landedOnTop =
                Mathf.Abs(contact.point.y - platformTop) < 0.2f
                && Mathf.Abs(contact.normal.y) > 0.7f;

            if (landedOnTop)
            {
                completed = true;

                if (tutorialText != null)
                {
                    tutorialText.text =
                        "Press Space to reach the higher platform!";

                    tutorialText.gameObject.SetActive(true);
                }

                Debug.Log("Long Jump tutorial completed!");

                break;
            }
        }
    }
}

