
using UnityEngine;

public class LavaController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Lava Speed")]
    [SerializeField] private float riseSpeed = 1.8f;
    [SerializeField] private float maximumRiseSpeed = 6f;
    [SerializeField] private float speedIncreasePerSecond = 0.02f;
    [SerializeField] private float acceleration = 2.5f;

    [Header("Chase Settings")]
    [SerializeField] private float catchUpDistance = 6f;
    [SerializeField] private float catchUpMultiplier = 0.7f;

    [Header("Starting Settings")]
    [SerializeField] private float startDelay = 4f;

    public float StartDelay => startDelay;

    private Rigidbody2D rb;
    private float elapsedTime;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentSpeed = riseSpeed;
    }

    private void FixedUpdate()
    {
        if (GameFlow.Instance != null &&
            GameFlow.Instance.IsEnded)
        {
            return;
        }

        if (player == null)
            return;

        elapsedTime += Time.fixedDeltaTime;

        if (elapsedTime < startDelay)
            return;

        float climbingTime = elapsedTime - startDelay;

        float targetSpeed = riseSpeed +
            climbingTime * speedIncreasePerSecond;

        float lavaHeight = rb != null
            ? rb.position.y
            : transform.position.y;

        float distance = player.position.y - lavaHeight;

        if (distance > catchUpDistance)
        {
            float extraDistance = distance - catchUpDistance;

            targetSpeed += extraDistance * catchUpMultiplier;
        }

        targetSpeed = Mathf.Clamp(
            targetSpeed,
            riseSpeed,
            maximumRiseSpeed
        );

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        Vector2 newPosition = new Vector2(
            transform.position.x,
            lavaHeight + currentSpeed * Time.fixedDeltaTime
        );

        if (rb != null)
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            transform.position = new Vector3(
                newPosition.x,
                newPosition.y,
                transform.position.z
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckPlayer(other);
    }

    private void CheckPlayer(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerController>() == null)
            return;

        if (GameFlow.Instance != null)
        {
            GameFlow.Instance.Lose();
        }
    }
}
