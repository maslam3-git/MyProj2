
using UnityEngine;

public class JumpStateEffect : StateMachineBehaviour
{
    private readonly Color jumpColor =
        new Color32(181, 124, 73, 255);

    private Color originalColor;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        SpriteRenderer sprite =
            animator.GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            originalColor = sprite.color;
            sprite.color = jumpColor;
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        SpriteRenderer sprite =
            animator.GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = originalColor;
        }
    }
}
