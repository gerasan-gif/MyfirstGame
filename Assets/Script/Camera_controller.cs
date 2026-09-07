using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float xOffset = 0.0f;

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + xOffset,
            fixedY,
            fixedZ
        );
    }
}