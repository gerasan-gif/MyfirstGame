using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAttack : MonoBehaviour
{
    private bool isPunching = false;
    private bool isKicking = false;

    private Rigidbody rb;
    private Animator animator;
    private PlayerMovement playerMovement;
    private GameManager gameManager;

    // PlayerMovement側から「キック中は移動しない」を判定するために公開
    public bool IsKicking => isKicking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // ゲームクリア or ゲームオーバー時はリターン
        if (gameManager != null &&
        (gameManager.IsStageClear || gameManager.IsGameOver))
        {
            return;
        }

        // パンチを出す
        if (Keyboard.current.pKey.wasPressedThisFrame && !isPunching)
        {
            StartCoroutine(PunchSequence());
        }

        // キックを出す
        if (Keyboard.current.kKey.wasPressedThisFrame && !isKicking)
        {
            if (playerMovement != null && playerMovement.IsGrounded)
            {
                // 地上時はキック
                StartCoroutine(KickSequence());
            }
            else
            {
                // ジャンプ時はジャンプキック
                StartCoroutine(JumpKickSequence());
            }
        }
    }

    IEnumerator PunchSequence()
    {
        isPunching = true;

        animator.speed = 1f;

        // 40%地点からパンチ開始
        animator.Play("Punch", 0, 0.4f);

        // パンチ＋腕を戻すまで
        yield return new WaitForSeconds(1.0f);

        // その構えで止める
        animator.speed = 0f;

        // 0.3秒ほど構えを維持
        yield return new WaitForSecondsRealtime(0.3f);

        // 再びAnimatorを動かす
        animator.speed = 1f;

        // Idleへ滑らかに戻す
        animator.CrossFade("Idle", 0.15f);

        isPunching = false;
    }

    IEnumerator KickSequence()
    {
        isKicking = true;

        animator.speed = 1f;
        animator.Play("Kick", 0, 0f);

        yield return new WaitForSeconds(1.0f);

        animator.CrossFade("Idle", 0.15f);

        isKicking = false;
    }

    IEnumerator JumpKickSequence()
    {
        isKicking = true;

        animator.speed = 1f;
        animator.Play("JumpKick", 0, 0f);

        // 現在向いている方向を取得
        Vector3 kickDirection = transform.forward;

        // 向いている方向へライダーキック
        float kickSpeed = 8f;

        rb.linearVelocity = new Vector3(
            kickDirection.x * kickSpeed,
            rb.linearVelocity.y,
            rb.linearVelocity.z
        );

        // 着地するまで待つ
        if (playerMovement != null)
        {
            while (!playerMovement.IsGrounded)
            {
                yield return null;
            }
        }

        // 着地後Idleへ
        animator.CrossFade("Idle", 0.15f);

        isKicking = false;
    }
}
