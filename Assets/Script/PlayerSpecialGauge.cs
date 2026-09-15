using UnityEngine;

public class PlayerSpecialGauge : MonoBehaviour
{
    [Header("Special Gauge")]
    [SerializeField] private int maxGauge = 100;

    private int currentGauge = 0;

    public int CurrentGauge => currentGauge;
    public int MaxGauge => maxGauge;
    public bool IsFull => currentGauge >= maxGauge;

    [SerializeField] private SoundEffectPlayer soundEffectPlayer;

    // ゲージを増やす
    public void AddGauge(int amount)
    {
        currentGauge += amount;
        currentGauge = Mathf.Clamp(currentGauge, 0, maxGauge);

        Debug.Log("必殺技ゲージ: " + currentGauge);

        if (currentGauge>=maxGauge){
            soundEffectPlayer.PlayGaugeFull();
        }
    }

    // 必殺技使用
    public bool UseGauge()
    {
        if (!IsFull)
        {
            Debug.Log("必殺技ゲージが足りません");
            return false;
        }

        currentGauge = 0;

        Debug.Log("必殺技ゲージ消費！");
        return true;
    }
}