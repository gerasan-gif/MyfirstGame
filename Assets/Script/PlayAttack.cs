using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Jump Kick")]
    public float jumpKickSpeed = 8f;

    [Header("References")]
    public PlayerMovement playerMovement;
    public PlayerAnimationController playerAnimation;
    public GameManager gameManager;
    public AttackHitBox kickHitBox;
    public AttackHitBox punchHitBox;

    private Rigidbody rb;
    private bool isPunching = false;
    private bool isKicking = false;

    public bool IsPunching => isPunching;
    public bool IsKicking => isKicking;
    public bool IsAttacking => isPunching || isKicking;

    private PlayerHealth playerHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        if (playerAnimation == null)
            playerAnimation = GetComponent<PlayerAnimationController>();

        if (kickHitBox == null)
            kickHitBox = GetComponentInChildren<AttackHitBox>(true);
    }

    void Start()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        if (kickHitBox != null)
            kickHitBox.DisableHitBox();
    }

    void Update()
    {
        // ダウン時は抜ける
        if (playerHealth != null && playerHealth.IsDead)
            return;

        if (Keyboard.current == null) return;

        if (gameManager != null &&
            (gameManager.IsStageClear || gameManager.IsGameOver))
            return;

        if (Keyboard.current.pKey.wasPressedThisFrame && !isPunching)
            StartCoroutine(PunchSequence());

        if (Keyboard.current.kKey.wasPressedThisFrame && !isKicking)
        {
            if (playerMovement != null && playerMovement.IsGrounded)
                StartCoroutine(KickSequence());
            else
                StartCoroutine(JumpKickSequence());
        }
    }

    IEnumerator PunchSequence()
    {
        Debug.Log("パンチ開始");

        isPunching = true;

        if (playerAnimation != null)
            playerAnimation.PlayPunch();

        // パンチが前へ出るタイミングまで待つ
        yield return new WaitForSeconds(0.2f);

        Debug.Log("PunchHitBoxをONにする");

        if (punchHitBox != null)
        {
            punchHitBox.EnableHitBox();
        }
        else{
            Debug.Log("punchHitBox が NULL！");
        }

        // 攻撃判定を短時間だけ有効化
        yield return new WaitForSeconds(0.2f);

        if (punchHitBox != null)
        {
            punchHitBox.DisableHitBox();
        }

        // アニメーションの残り
        yield return new WaitForSeconds(0.4f);

        if (playerAnimation != null)
         {
            playerAnimation.PauseAnimation();
        }

        yield return new WaitForSecondsRealtime(0.3f);

        if (playerAnimation != null)
        {
            playerAnimation.ResumeAnimation();
            playerAnimation.PlayIdle();
        }

        isPunching = false;
    }

    IEnumerator KickSequence()
    {
        isKicking = true;

        if (playerAnimation != null)
            playerAnimation.PlayKick();

        yield return new WaitForSeconds(0.60f);

        if (kickHitBox != null)
            kickHitBox.EnableHitBox();

        yield return new WaitForSeconds(0.25f);

        if (kickHitBox != null)
            kickHitBox.DisableHitBox();

        yield return new WaitForSeconds(0.55f);

        if (playerAnimation != null)
            playerAnimation.PlayIdle();

        isKicking = false;
    }

    IEnumerator JumpKickSequence()
    {
        isKicking = true;

        if (playerAnimation != null)
            playerAnimation.PlayJumpKick();

        Vector3 kickDirection = transform.forward;
        kickDirection.y = 0f;

        if (kickDirection.sqrMagnitude > 0f)
            kickDirection.Normalize();

        rb.linearVelocity = new Vector3(
            kickDirection.x * jumpKickSpeed,
            -1.5f,
            rb.linearVelocity.z
        );

        // 10フレーム付近まで待つ
        yield return new WaitForSeconds(0.30f);

        if (kickHitBox != null)
            kickHitBox.EnableHitBox();

        // さらに20フレーム付近まで待つ
        yield return new WaitForSeconds(0.40f);

        // 着地待ち
        if (playerMovement != null)
        {
            while (!playerMovement.IsGrounded)
                yield return null;
        }

        if (kickHitBox != null)
            kickHitBox.DisableHitBox();

        if (playerAnimation != null)
            playerAnimation.PlayIdle();

        isKicking = false;
    }
}
