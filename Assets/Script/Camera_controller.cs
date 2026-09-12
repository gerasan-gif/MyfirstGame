using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float xOffset = 0.0f;

    private float fixedY;
    private float fixedZ;

    private bool stopFollow = false;

    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        stopFollow = false;
    }

    void LateUpdate()
    {
        if (stopFollow) return;

        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + xOffset,
            fixedY,
            fixedZ
        );
    }
}