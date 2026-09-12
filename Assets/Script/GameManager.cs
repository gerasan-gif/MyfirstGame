using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public Rigidbody playerRb;
    public PlayerAnimationController playerAnimation;

    [Header("Stage Clear")]
    public float stageClearX = -100f;
    public GameObject stageClearUI;

    [Header("Game Over")]
    public float gameOverY = -50f;
    public GameObject gameOverUI;

    private bool isStageClear = false;
    private bool isGameOver = false;

    public bool IsStageClear => isStageClear;
    public bool IsGameOver => isGameOver;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (isStageClear || isGameOver)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                RestartScene();
            }

            return;
        }

        if (player == null)
            return;

        if (player.position.x < stageClearX)
        {
            StageClear();
        }

        if (player.position.y < gameOverY)
        {
            GameOver();
        }
    }

    void StageClear()
    {
        if (isStageClear)
            return;

        isStageClear = true;

        Debug.Log("STAGE CLEAR!");

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
        }

        if (playerAnimation != null)
        {
            playerAnimation.PlayIdle();
        }

        if (stageClearUI != null)
        {
            stageClearUI.SetActive(true);
        }

        StartCoroutine(ClearDanceSequence());
    }

    IEnumerator ClearDanceSequence()
    {
        yield return new WaitForSeconds(1.5f);

        if (player != null)
        {
            player.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (playerAnimation != null)
        {
            playerAnimation.PlayDance();
        }
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        Debug.Log("GAME OVER");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
        }

        if (playerAnimation != null)
        {
            playerAnimation.StopAnimation();
        }

        Time.timeScale = 0f;
    }

    void RestartScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
