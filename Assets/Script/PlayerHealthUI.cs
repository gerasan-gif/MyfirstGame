using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private TMP_Text heart1;
    [SerializeField] private TMP_Text heart2;
    [SerializeField] private TMP_Text heart3;

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