using UnityEngine;

public class HeartProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 2;
    public float lifeTime = 2f;

    [Header("Hit Effect")]
    [SerializeField] private GameObject hitEffectPrefab;

    [Header("Sound")]
    [SerializeField] private AudioClip hitSE;

    private Rigidbody rb;

    private Vector3 moveDirection = Vector3.right;

    [SerializeField] private AttackHitBox.AttackType attackType = AttackHitBox.AttackType.Special;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    public void Launch(Vector3 direction)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;
        }
    }

    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage, attackType);
            SpawnHitEffect();
        
            // 着弾SE
            if (hitSE != null)
            {
                AudioSource.PlayClipAtPoint(
                hitSE,
                transform.position,
                1f
                );
            }
            
            Destroy(gameObject);
            return;
        }

        // 地面や障害物に当たったら消すなら
        if (other.CompareTag("Ground") || other.CompareTag("Obstacle"))
        {
            SpawnHitEffect();
            Destroy(gameObject);
        }
    }

    void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                hitEffectPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, 1f);
        }
    }
}
