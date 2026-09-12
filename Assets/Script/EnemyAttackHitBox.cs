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