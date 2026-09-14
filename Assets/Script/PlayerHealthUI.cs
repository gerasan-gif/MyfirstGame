using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    void Update()
    {
        int hp = playerHealth.CurrentHp;

        heart1.enabled = hp >= 1;
        heart2.enabled = hp >= 2;
        heart3.enabled = hp >= 3;
    }
}