using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private EnemyAttackHitBox attackHitBox;

    public void EnablePunchHitBox()
    {
        if (attackHitBox != null)
            attackHitBox.EnablePunchHitBox();
    }

    public void DisablePunchHitBox()
    {
        if (attackHitBox != null)
            attackHitBox.DisablePunchHitBox();
    }
}
