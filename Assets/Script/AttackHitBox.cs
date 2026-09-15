using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
    [Header("Damage")]
    public int damage = 1;

    private Collider hitBoxCollider;
    private bool canHit = false;

    public enum AttackType
    {
        Normal,
        Special
    }

    public enum AttackSoundType
    {
        Punch,
        Kick,
        Jumpkick
    }

    public AttackType attackType = AttackType.Normal;
    public AttackSoundType attackSoundType = AttackSoundType.Punch;
    private PlayerAttack playerAttack;

    void Awake()
    {
        hitBoxCollider = GetComponent<Collider>();
        hitBoxCollider.enabled = false;
        playerAttack = GetComponentInParent<PlayerAttack>();
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

        Debug.Log("HitBox 接触: " + other.gameObject.name);

        if (!canHit) 
        {
            Debug.Log("接触したが HitBox は無効時間");
            return;
        }

        EnemyController enemy = other.GetComponentInParent<EnemyController>();

        if (enemy != null) 
        {
            // 戦闘員がパンチ攻撃中か確認
            EnemyAttackController enemyAttack =
                enemy.GetComponent<EnemyAttackController>();

            if (enemyAttack != null && 
                enemyAttack.IsPunchActive &&
                attackType == AttackType.Normal)
            {
                Debug.Log("戦闘員のパンチが優先！風香の通常攻撃は無効");
                return;
            }

            Debug.Log("敵に命中！");
            enemy.TakeDamage(damage);

            switch (attackSoundType)
            {
                case AttackSoundType.Punch:
                    soundEffectPlayer.PlayPunchHit();
                    playerAttack.AddPunchGauge();
                    break;

                case AttackSoundType.Kick:
                    soundEffectPlayer.PlayKickHit();
                    playerAttack.AddKickGauge();
                    break;

                case AttackSoundType.Jumpkick:
                    soundEffectPlayer.PlayJumpkickHit();
                    break;
            }

        }else{
            Debug.Log("EnemyController が見つからない");
            return;
        }

        // 1回の攻撃で同じ敵に連続ヒットしないようにする
        DisableHitBox();
    }
}
