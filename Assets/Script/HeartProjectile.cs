using UnityEngine;

public class HeartProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 2;
    public float lifeTime = 2f;

    private Vector3 moveDirection = Vector3.right;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
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
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 地面や障害物に当たったら消すなら
        if (other.CompareTag("Ground") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
