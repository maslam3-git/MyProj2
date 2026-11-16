using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PortalExit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer portalImage;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.green;

    private void Update()
    {
        if (portalImage != null && PuzzleController.Instance != null)
            portalImage.color = PuzzleController.Instance.IsComplete ? unlockedColor : lockedColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PuzzleController.Instance != null &&
            PuzzleController.Instance.IsComplete && GameFlow.Instance != null)
        {
            GameFlow.Instance.Win();
        }
    }
}
