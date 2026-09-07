using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Chara_test_controller : MonoBehaviour
{
    private float startX;

    public float speed = 3f;
    public float jumpPower = 5f;
    private bool isPunching = false;
    private bool isKicking = false;

    public float stageClearY = -100f;
    public GameObject stageClearUI;
    private bool isStageClear = false;

    public float gameOverY = -50f;
    public GameObject gameOverUI;
    private bool isGameOver = false;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded = true;

    void Start()
    {
        startX = transform.position.x;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        // 初期位置より逆にいかない
        if (transform.position.x > startX)
        {
            Vector3 pos = transform.position;
            pos.x = startX;
            transform.position = pos;
        }

        // ステージクリア中
        if (isStageClear)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;

                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex
                );
            }

            return;
        }

        // ゲームオーバー中
        if (isGameOver)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;

                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex
                );
            }

            return;
        }

        float move = 0f;

        if (Keyboard.current.aKey.isPressed){
            move = 1f;

            // 反対向き
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (Keyboard.current.dKey.isPressed){
            move = -1f;

            // 前向き
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));


        if (!isKicking)
        {
            transform.Translate(
                new Vector3(move, 0, 0) * speed * Time.deltaTime,
                Space.World
            );
        }

        //　パンチを出す
        if (Keyboard.current.pKey.wasPressedThisFrame && !isPunching)
        {
            StartCoroutine(PunchSequence());
        }

        //　キックを出す
        if (Keyboard.current.kKey.wasPressedThisFrame && !isKicking)
        {
             if (isGrounded) // 地上時はハイキック
            {
                StartCoroutine(KickSequence());
            }
            else //ジャンプ時はジャンプキック
            {
                StartCoroutine(JumpKickSequence());
            }
        }

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

            // ★走るアニメーションを停止
            animator.speed = 0f;
        }

        //ゲームクリア
        if (transform.position.x < stageClearY)
        {
            StageClear();
        }

        //ゲームオーバー
        if (!isGameOver && transform.position.y < gameOverY)
        {
            GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // ★走るアニメーションを再開
            animator.speed = 1f;
        }
    }

    void StageClear()
    {
        isStageClear = true;

        Debug.Log("STAGE CLEAR!");

        // 移動を止める
        rb.linearVelocity = Vector3.zero;

        // Animator自体は動かしておく
        animator.speed = 1f;
        animator.SetFloat("Speed", 0f);

        // Speedを0にして Idle へ遷移
        animator.SetFloat("Speed", 0f);

        if (stageClearUI != null)
        {
            stageClearUI.SetActive(true);
        }
        StartCoroutine(ClearDanceSequence());
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

        // 0.4秒ほど構えを維持
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
        while (!isGrounded)
        {
            yield return null;
        }

        // 着地後Idleへ
        animator.CrossFade("Idle", 0.15f);

        isKicking = false;
    }

    IEnumerator ClearDanceSequence()
    {
        // まずIdleで少し待つ
        yield return new WaitForSeconds(1.5f);

        // カメラ側を向く
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        // ダンス開始
        animator.speed = 1f;
        animator.Play("Dance", 0, 0f);
    }

    void GameOver()
    {
        isGameOver = true;

        Debug.Log("GAME OVER");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        animator.speed = 0f;
        rb.linearVelocity = Vector3.zero;

        Time.timeScale = 0f;
    }  
}