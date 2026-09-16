using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Heart Images")]
    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;

    void Update()
    {
        if (playerHealth == null)
            return;

        int hp = playerHealth.CurrentHp;

        heart1.enabled = hp >= 1;
        heart2.enabled = hp >= 2;
        heart3.enabled = hp >= 3;
    }
}