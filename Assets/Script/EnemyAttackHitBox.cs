using UnityEngine;

public class EnemyAttackHitBox : MonoBehaviour
{
    private Collider hitBoxCollider;
    private bool hasHit = false;

    void Awake()
    {
        hitBoxCollider = GetComponent<Collider>();

        if (hitBoxCollider != null)
            hitBoxCollider.enabled = false;
    }

    public void EnablePunchHitBox()
    {
        hasHit = false;   // 新しいパンチなので命中状態をリセット

        if (hitBoxCollider != null)
            hitBoxCollider.enabled = true;
    }

    public void DisablePunchHitBox()
    {
        if (hitBoxCollider != null)
            hitBoxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerAttack playerAttack =
        other.GetComponentInParent<PlayerAttack>();

        // ライダーキック中は戦闘員のパンチを無効化
        if (playerAttack != null && playerAttack.IsJumpKicking)
        {
            Debug.Log("ジャンプキック中！戦闘員のパンチは無効");
            return;
        }

        // このパンチですでに命中していたら何もしない
        if (hasHit)
            return;

        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            hasHit = true;

            Debug.Log("戦闘員の攻撃が風香に命中！");
            playerHealth.TakeDamage(1);
        }
    }
}