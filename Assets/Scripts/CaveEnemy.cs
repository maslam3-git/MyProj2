
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CaveEnemy : MonoBehaviour
{
    public float moveDistance = 4f;
    public float moveSpeed = 2f;
    public int damage = 1;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float movementTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        startPosition = rb.position;
    }

    private void FixedUpdate()
    {
        movementTime += Time.fixedDeltaTime;

        float offset =
            Mathf.Sin(movementTime * moveSpeed) *
            moveDistance;

        Vector2 target = new Vector2(
            startPosition.x + offset,
            startPosition.y
        );

        rb.MovePosition(target);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
