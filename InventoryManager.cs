using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public TMP_Text itemCountText;

    public TMP_Text keyCountText;

    private int itemCount = 0;

    private int keyCount = 0;

    void RefreshKeyUI()
    {
        if (keyCountText != null)
        {
            keyCountText.text = "Keys: " + keyCount;
        }
    }

    public void AddKey()
    {
        keyCount++;

        RefreshKeyUI();

        Debug.Log("获得钥匙，当前数量：" + keyCount);
    }

    public bool HasKey()
    {
        return keyCount > 0;
    }

    public bool UseKey()
    {
        if (keyCount <= 0)
        {
            return false;
        }

        keyCount--;

        RefreshKeyUI();

        Debug.Log("使用钥匙，剩余数量：" + keyCount);

        return true;
    }

    void Start()
    {
        UpdateUI();

        RefreshKeyUI();
    }

    public void AddItem()
    {
        itemCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        itemCountText.text = "Items: " + itemCount;
    }
}

