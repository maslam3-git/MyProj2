
using UnityEngine;

public class PlatformColor : MonoBehaviour
{
    [SerializeField]
    private Color[] colors =
    {
        new Color32(86, 80, 91, 255),
        new Color32(134, 99, 74, 255),
        new Color32(173, 130, 96, 255),
        new Color32(104, 119, 131, 255)
    };

    private void Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null && colors.Length > 0)
        {
            sprite.color = colors[Random.Range(0, colors.Length)];
        }
    }
}
