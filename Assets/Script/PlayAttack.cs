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
    public AttackHitBox jumpkickHitBox;

    [SerializeField] private SoundEffectPlayer soundEffectPlayer;

    private Rigidbody rb;
    private bool isPunching = false;
    private bool isKicking = false;
    private bool isJumpKicking = false;

    public bool IsPunching => isPunching;
    public bool IsKicking => isKicking;
    public bool IsAttacking => isPunching || isKicking;
    public bool IsJumpKicking => isJumpKicking;

    private PlayerHealth playerHealth;

    [Header("Special Gauge")]
    [SerializeField] private PlayerSpecialGauge specialGauge;

    [SerializeField] private int punchGaugeAmount = 10;
    [SerializeField] private int kickGaugeAmount = 15;

    public void CancelAttack()
    {
        // PlayAttack内で動いている攻撃コルーチンを停止
        StopAllCoroutines();

        // 攻撃状態を解除
        isPunching = false;
        isKicking = false;
        isJumpKicking = false;

        // 攻撃判定も消す
        if (punchHitBox != null)
            punchHitBox.DisableHitBox();

        if (kickHitBox != null)
            kickHitBox.DisableHitBox();

        if (jumpkickHitBox != null)
            jumpkickHitBox.DisableHitBox();
    }

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

        if (jumpkickHitBox == null)
            jumpkickHitBox = GetComponentInChildren<AttackHitBox>(true);

        if (specialGauge == null)
        {
            specialGauge = GetComponent<PlayerSpecialGauge>();
        }
    }

    void Start()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        if (kickHitBox != null)
            kickHitBox.DisableHitBox();

        if (jumpkickHitBox != null)
            jumpkickHitBox.DisableHitBox();
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

        if (Keyboard.current.pKey.wasPressedThisFrame && !isPunching && playerMovement.IsGrounded)
            StartCoroutine(PunchSequence());

        if (Keyboard.current.kKey.wasPressedThisFrame && !isKicking)
        {
            if (playerMovement != null && playerMovement.IsGrounded)
                StartCoroutine(KickSequence());
            else
                if (!specialGauge.UseGauge()){
                    return;
                }else{
                    StartCoroutine(JumpKickSequence());
                }
        }
    }

    public void AddPunchGauge()
    {
        if (specialGauge != null)
        {
            specialGauge.AddGauge(punchGaugeAmount);
        }
    }

    public void AddKickGauge()
    {
        if (specialGauge != null)
        {
            specialGauge.AddGauge(kickGaugeAmount);
        }
    }

    IEnumerator PunchSequence()
    {
        Debug.Log("パンチ開始");

        isPunching = true;

        // ★パンチは通常攻撃
        punchHitBox.attackType = AttackHitBox.AttackType.Normal;

        if (playerAnimation != null)
            playerAnimation.PlayPunch();

        // パンチが前へ出るタイミングまで待つ
        yield return new WaitForSeconds(0.2f);

        // 空振り音を鳴らす
        soundEffectPlayer.PlayPunchkMiss();

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

        // ★地上キックは通常攻撃
        kickHitBox.attackType = AttackHitBox.AttackType.Normal;

        if (playerAnimation != null)
            playerAnimation.PlayKick();

        yield return new WaitForSeconds(0.60f);

        // 空振り音を鳴らす
        soundEffectPlayer.PlayKickMiss();

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
        isJumpKicking = true;

        // ★ジャンプキックは強攻撃
        kickHitBox.attackType = AttackHitBox.AttackType.Special;

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

        if (jumpkickHitBox != null)
            jumpkickHitBox.EnableHitBox();

        // さらに20フレーム付近まで待つ
        yield return new WaitForSeconds(0.40f);

        // 着地待ち
        if (playerMovement != null)
        {
            while (!playerMovement.IsGrounded)
                yield return null;
        }

        if (jumpkickHitBox != null)
            jumpkickHitBox.DisableHitBox();

        if (playerAnimation != null)
            playerAnimation.PlayIdle();

        isKicking = false;
        isJumpKicking = false;
    }
}
