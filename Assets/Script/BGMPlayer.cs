using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public enum LoopMode
    {
        None,
        Normal,
        Custom
    }

    [Header("BGM")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;

    [Header("Loop Settings")]
    [SerializeField] private LoopMode loopMode = LoopMode.Normal;

    [Tooltip("Custom Loop の開始位置（秒）")]
    [SerializeField] private float loopStartTime = 0f;

    [Tooltip("Custom Loop の終了位置（秒）。0なら曲の最後")]
    [SerializeField] private float loopEndTime = 0f;

    private AudioSource audioSource;

    private int loopStartSample;
    private int loopEndSample;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0f;
    }

    void Start()
    {
        if (bgmClip == null)
        {
            Debug.LogWarning("BGM Clip が設定されていません。");
            return;
        }

        audioSource.clip = bgmClip;

        SetupLoop();

        audioSource.Play();
    }

    void Update()
    {
        if (loopMode != LoopMode.Custom)
            return;

        if (audioSource.timeSamples >= loopEndSample - 1000)
        {
            audioSource.timeSamples = loopStartSample;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    private void SetupLoop()
    {
        switch (loopMode)
        {
            case LoopMode.None:
                audioSource.loop = false;
                break;

            case LoopMode.Normal:
                audioSource.loop = true;
                break;

            case LoopMode.Custom:
                audioSource.loop = false;

                loopStartSample =
                    Mathf.RoundToInt(loopStartTime * bgmClip.frequency);

                if (loopEndTime <= 0f)
                {
                    loopEndSample = bgmClip.samples;
                }
                else
                {
                    loopEndSample =
                        Mathf.RoundToInt(loopEndTime * bgmClip.frequency);
                }

                // 念のため範囲内に収める
                loopStartSample =
                    Mathf.Clamp(loopStartSample, 0, bgmClip.samples - 1);

                loopEndSample =
                    Mathf.Clamp(loopEndSample, loopStartSample + 1, bgmClip.samples);
                break;
        }
    }

    public void ChangeBGM(AudioClip clip, bool loop)
    {
        if (clip == null)
            return;

        audioSource.Stop();

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.timeSamples = 0;

        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlayBGM()
    {
        if (bgmClip != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }
}