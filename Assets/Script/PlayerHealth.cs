using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHp = 3;

    private int currentHp;
    private bool isDead = false;
    private bool isHit = false;

    private PlayerAnimationController playerAnimation;
    [SerializeField] private Transform deathCameraTarget;

    public int CurrentHp => currentHp;
    public bool IsDead => isDead;

    void Awake()
    {
        currentHp = maxHp;

        playerAnimation = GetComponent<PlayerAnimationController>();
    }

    public void TakeDamage(int damage)
    {
        // 死亡後はダメージを受けない
        if (isDead)
            return;

        currentHp -= damage;

        if (currentHp < 0)
            currentHp = 0;

        Debug.Log("風香 HP = " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitReactionSequence());
        }
    }

    IEnumerator HitReactionSequence()
    {
        // 連続でリアクションを出さない
        if (isHit)
            yield break;

        isHit = true;

        if (playerAnimation != null)
            playerAnimation.PlayHitReaction();

        // HitReactionの長さに合わせて後で調整
        yield return new WaitForSeconds(0.6f);

        if (!isDead && playerAnimation != null)
            playerAnimation.PlayIdle();

        isHit = false;
    }

    void Die()
    {
        isDead = true;

        Debug.Log("PLAYER DOWN");

        if (playerAnimation != null)
            playerAnimation.PlayDead();

        CameraFollow cameraController =
            Camera.main.GetComponent<CameraFollow>();

        if (cameraController != null && deathCameraTarget != null)
        {
            cameraController.SetTarget(deathCameraTarget);
        }
        // GameManagerのGameOverにつなぐ
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(3.0f);

        GameManager gameManager =
            FindAnyObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}
