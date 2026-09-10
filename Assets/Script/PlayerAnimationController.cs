using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 移動速度に応じてIdle / Runningを切り替える
    public void SetMoveSpeed(float move)
    {
        animator.SetFloat("Speed", Mathf.Abs(move));
    }

    // ジャンプ時のポーズ
    public void PlayJumpPose()
    {
        animator.speed = 1f;

        // 元のChara_test_controllerと同じく、
        // Runningの0.1地点を表示して停止する
        animator.Play("Running", 0, 0.1f);
        animator.speed = 0f;
    }

    // 着地時にアニメーションを再開
    public void ResumeAfterLanding()
    {
        animator.speed = 1f;
    }

    // パンチ
    public void PlayPunch()
    {
        animator.speed = 1f;

        // 40%地点からパンチ開始
        animator.Play("Punch", 0, 0.4f);
    }

    // 地上キック
    public void PlayKick()
    {
        animator.speed = 1f;
        animator.Play("Kick", 0, 0f);
    }

    // ジャンプキック
    public void PlayJumpKick()
    {
        animator.speed = 1f;
        animator.Play("JumpKick", 0, 0f);
    }

    // Idleへ戻す
    public void PlayIdle()
    {
        animator.speed = 1f;
        animator.SetFloat("Speed", 0f);
        animator.CrossFade("Idle", 0.15f);
    }

    // ステージクリア時のダンス
    public void PlayDance()
    {
        animator.speed = 1f;
        animator.Play("Dance", 0, 0f);
    }

    // Animatorを一時停止
    public void PauseAnimation()
    {
        animator.speed = 0f;
    }

    // Animatorを再開
    public void ResumeAnimation()
    {
        animator.speed = 1f;
    }

    // ゲームオーバーなどでAnimatorを停止
    public void StopAnimation()
    {
        animator.speed = 0f;
    }
}
