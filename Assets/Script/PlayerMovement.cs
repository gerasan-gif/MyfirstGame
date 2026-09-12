using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float startX;

    public float speed = 3f;
    public float jumpPower = 5f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded = true;

    // PlayerAttack から参照するため公開
    public bool IsGrounded => isGrounded;

    private GameManager gameManager;
    private PlayerHealth playerHealth;

    void Start()
    {
        startX = transform.position.x;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        // ダウン時は抜ける
        if (playerHealth != null && playerHealth.IsDead)
            return;

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

        // 初期位置より逆にいかない
        if (transform.position.x > startX)
        {
            Vector3 pos = transform.position;
            pos.x = startX;
            transform.position = pos;
        }

        float move = 0f;

        if (Keyboard.current.aKey.isPressed)
        {
            move = 1f;

            // 反対向き
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (Keyboard.current.dKey.isPressed)
        {
            move = -1f;

            // 前向き
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));

        transform.Translate(
            new Vector3(move, 0f, 0f) * speed * Time.deltaTime,
            Space.World
        );

        // ジャンプ
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            isGrounded = false;

            // 毎回同じポーズにする
            animator.Play(
                "Running",
                0,
                0.1f
            );

            // 走るアニメーションを停止
            animator.speed = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // 走るアニメーションを再開
            animator.speed = 1f;
        }
    }

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
}
