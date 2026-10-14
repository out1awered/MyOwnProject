using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class WallJumpFinishDetector : MonoBehaviour
{
    public TMP_Text tutorialText;

    private bool completed = false;
    private Collider platformCollider;

    void Start()
    {
        platformCollider = GetComponent<Collider>();
    }

    void OnCollisionStay(Collision collision)
    {
        // 已完成检测，不再重复执行
        if (completed)
        {
            return;
        }

        // 检查是否为玩家
        PlayerMovement player =
            collision.gameObject.GetComponentInParent<PlayerMovement>();

        if (player == null || platformCollider == null)
        {
            return;
        }

        // 获取平台顶部的高度
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
                    // 只隐藏蹬墙跳提示
                    if (tutorialText.text.ToLower().Contains("wall jump"))
                    {
                        tutorialText.gameObject.SetActive(false);
                    }
                }

                Debug.Log("Wall Jump Platform Reached!");

                break;
            }
        }
    }
}

