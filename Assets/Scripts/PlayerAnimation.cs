
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.18f;

    private SpriteRenderer playerSprite;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator != null)
            playerSprite = animator.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (animator == null || rb == null ||
            groundCheck == null)
            return;

        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        float verticalSpeed = rb.linearVelocity.y;

        bool grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        ) != null;

        animator.SetFloat("Speed", Mathf.Clamp01(horizontalSpeed / 6f));
        animator.SetFloat("VerticalSpeed", verticalSpeed);
        animator.SetBool("Grounded", grounded);

        if (playerSprite != null)
        {
            if (rb.linearVelocity.x > 0.1f)
                playerSprite.flipX = false;
            else if (rb.linearVelocity.x < -0.1f)
                playerSprite.flipX = true;
        }
    }
}
