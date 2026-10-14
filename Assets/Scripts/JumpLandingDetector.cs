using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using TMPro;

public class JumpLandingDetector : MonoBehaviour
{
    public TMP_Text tutorialText;

    // 引用冲刺教学脚本
    public SprintTutorialZone sprintTutorial;

    private bool completed = false;

    void OnCollisionEnter(Collision collision)
    {
        if (completed)
        {
            return;
        }

        if (collision.gameObject.GetComponentInParent<PlayerMovement>() == null)
        {
            return;
        }

        float platformTop = GetComponent<Collider>().bounds.max.y;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (Mathf.Abs(contact.point.y - platformTop) < 0.15f
                && Mathf.Abs(contact.normal.y) > 0.7f)
            {
                completed = true;

                Debug.Log("Jump tutorial completed!");

                // 通知冲刺教学开始
                if (sprintTutorial != null)
                {
                    sprintTutorial.BeginSprintTutorial();
                }

                break;
            }
        }
    }
}

