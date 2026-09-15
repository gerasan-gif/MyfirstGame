using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialGaugeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerSpecialGauge specialGauge;
    [SerializeField] private Slider gaugeSlider;

    void Start()
    {
        if (specialGauge == null || gaugeSlider == null)
            return;

        gaugeSlider.minValue = 0;
        gaugeSlider.maxValue = specialGauge.MaxGauge;
        gaugeSlider.value = specialGauge.CurrentGauge;
    }

    void Update()
    {
        if (specialGauge == null || gaugeSlider == null)
            return;

        gaugeSlider.value = specialGauge.CurrentGauge;
    }
}