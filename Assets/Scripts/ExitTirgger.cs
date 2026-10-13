using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public GameObject winPanel;

    public GameObject startPanel;

    public void StartGame()
    {
        if (currentState != GameState.Ready)
        {
            return;
        }

        currentState = GameState.Playing;

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("当前游戏状态：" + currentState);
    }

    private enum GameState
    {
        Ready,
        Playing,
        Won
    }

    private GameState currentState = GameState.Ready;

    void Start()
    {
        currentState = GameState.Ready;

        Time.timeScale = 0f;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("当前游戏状态：" + currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
    currentState == GameState.Playing)
        {
            currentState = GameState.Won;

            Debug.Log("当前游戏状态：" + currentState);

            Debug.Log("玩家通关！");

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restart 按钮被点击！");

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}