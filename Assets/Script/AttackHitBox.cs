using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitBox : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;

    private Collider hitBoxCollider;
    private bool canHit = false;

    void Awake()
    {
        hitBoxCollider = GetComponent<Collider>();
        hitBoxCollider.enabled = false;
    }

    public void EnableHitBox()
    {
        canHit = true;
        hitBoxCollider.enabled = true;
    }

    public void DisableHitBox()
    {
        canHit = false;
        hitBoxCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("KickHitBox 接触: " + other.gameObject.name);

        if (!canHit) 
        {
            Debug.Log("接触したが HitBox は無効時間");
            return;
        }

        EnemyController enemy = other.GetComponentInParent<EnemyController>();
        if (enemy == null) 
        {
            Debug.Log("EnemyController が見つからない");
            return;
        }

        Debug.Log("敵にキック命中！");
        enemy.TakeDamage(damage);

        // 1回の攻撃で同じ敵に連続ヒットしないようにする
        DisableHitBox();
    }
}
