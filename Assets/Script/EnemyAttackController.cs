using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private EnemyAttackHitBox attackHitBox;

    public bool IsPunchActive { get; private set; }

    public void EnablePunchHitBox()
    {
        IsPunchActive = true;
        if (attackHitBox != null)
            attackHitBox.EnablePunchHitBox();
    }

    public void DisablePunchHitBox()
    {
        IsPunchActive = false;
        if (attackHitBox != null)
            attackHitBox.DisablePunchHitBox();
    }
}
