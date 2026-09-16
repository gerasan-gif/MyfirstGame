using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private AudioClip punchHit;
    [SerializeField] private AudioClip kickHit;
    [SerializeField] private AudioClip jumpkickHit;
    [SerializeField] private AudioClip punchMiss;
    [SerializeField] private AudioClip kickMiss;
    [SerializeField] private AudioClip attackMiss;

    [Header("Player / Enemy")]
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip knockDown;

    [Header("System")]
    [SerializeField] private AudioClip gaugeFull;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    private void PlaySE(AudioClip clip)
    {
        if (clip == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayPunchHit()
    {
        PlaySE(punchHit);
    }

    public void PlayKickHit()
    {
        PlaySE(kickHit);
    }

    public void PlayJumpkickHit()
    {
        PlaySE(jumpkickHit);
    }

    public void PlayAttackMiss()
    {
        PlaySE(attackMiss);
    }

    public void PlayPunchkMiss()
    {
        PlaySE(punchMiss);
    }

    public void PlayKickMiss()
    {
        PlaySE(kickMiss);
    }

    public void PlayDamage()
    {
        PlaySE(damage);
    }

    public void PlayJump()
    {
        PlaySE(jump);
    }

    public void PlayLand()
    {
        PlaySE(land);
    }

    public void PlayKnockDown()
    {
        PlaySE(knockDown);
    }

    public void PlayGaugeFull()
    {
        PlaySE(gaugeFull);
    }
}