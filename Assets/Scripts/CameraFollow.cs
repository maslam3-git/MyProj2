
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float offsetY = 1.5f;
    [SerializeField] private float smoothSpeed = 4f;

    private float highestCameraY;

    private void Start()
    {
        highestCameraY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        float targetY = player.position.y + offsetY;

        highestCameraY = Mathf.Max(
            highestCameraY,
            targetY
        );

        Vector3 target = new Vector3(
            transform.position.x,
            highestCameraY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            target,
            smoothSpeed * Time.deltaTime
        );
    }
}
