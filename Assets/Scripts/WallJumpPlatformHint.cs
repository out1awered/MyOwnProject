using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class WallJumpPlatformHint : MonoBehaviour
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
        // 已经显示过提示，不再检测
        if (completed)
        {
            return;
        }

        // 检测碰撞对象是否为玩家
        PlayerMovement player =
            collision.gameObject.GetComponentInParent<PlayerMovement>();

        if (player == null)
        {
            return;
        }

        if (platformCollider == null)
        {
            return;
        }

        // 获取平台顶面的高度
        float platformTop = platformCollider.bounds.max.y;

        foreach (ContactPoint contact in collision.contacts)
        {
            // 判断玩家是否接触平台顶面
            bool landedOnTop =
                Mathf.Abs(contact.point.y - platformTop) < 0.2f
                && Mathf.Abs(contact.normal.y) > 0.7f;

            if (landedOnTop)
            {
                completed = true;

                if (tutorialText != null)
                {
                    tutorialText.text =
                        "Run toward the wall and press Space near it to perform a wall jump!";

                    tutorialText.gameObject.SetActive(true);
                }

                Debug.Log("Wall Jump Tutorial Started!");

                break;
            }
        }
    }
}

