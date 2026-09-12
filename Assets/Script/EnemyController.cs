using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy")]
    public int maxHp = 1;

    [Header("Animation")]
    public Animator animator;
    public string hitReactionState = "HitReaction";
    public string knockDownState = "Sentinel_Knock_Down";

    [Header("Defeat")]
    public float destroyDelay = 3.0f;

    private int currentHp;
    private bool isDefeated = false;

    public EnemyAttackHitBox enemyAttackHitBox;

    void Awake()
    {
        currentHp = maxHp;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDefeated) return;

        currentHp -= damage;

        Debug.Log(gameObject.name + " HIT! HP = " + currentHp);

        if (currentHp <= 0)
        {
            StartCoroutine(DefeatSequence());
        }
        else
        {
            PlayAnimationIfExists(hitReactionState);
        }
    }

    IEnumerator DefeatSequence()
    {
        isDefeated = true;

        Rigidbody rb = GetComponent<Rigidbody>();
 
        if (rb != null)
        {
            // 通常時のX固定を解除
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;

            // Z方向と回転は固定したまま
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
            rb.constraints |= RigidbodyConstraints.FreezeRotationX;
            rb.constraints |= RigidbodyConstraints.FreezeRotationY;
            rb.constraints |= RigidbodyConstraints.FreezeRotationZ;

            rb.linearVelocity = Vector3.zero;
        }

        // 吹き飛びアニメーション
        PlayAnimationIfExists(knockDownState);

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);

    }

    void PlayAnimationIfExists(string stateName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return;

        int stateHash = Animator.StringToHash(stateName);

        if (animator.HasState(0, stateHash))
            animator.Play(stateHash, 0, 0f);
    }
}

