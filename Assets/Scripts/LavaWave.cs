
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LavaWave : MonoBehaviour
{
    [SerializeField] private float waveSpeed = 5f;
    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private float waveHeight = 4f;

    private Texture2D texture;
    private Sprite waveSprite;
    private SpriteRenderer spriteRenderer;

    private const int width = 320;
    private const int height = 16;

    private Color32[] pixels;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        pixels = new Color32[width * height];

        waveSprite = Sprite.Create(
            texture,
            new Rect(0, 0, width, height),
            new Vector2(0.5f, 0f),
            16f
        );

        spriteRenderer.sprite = waveSprite;
        spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        float time = Time.time * waveSpeed;

        for (int x = 0; x < width; x++)
        {
            float wave = Mathf.Sin(
                x * waveFrequency * 0.05f - time
            ) * waveHeight;

            int surface = 9 + Mathf.RoundToInt(wave);

            for (int y = 0; y < height; y++)
            {
                int index = y * width + x;

                if (y > surface)
                {
                    pixels[index] = new Color32(0, 0, 0, 0);
                }
                else if (y >= surface - 1)
                {
                    pixels[index] = new Color32(255, 215, 90, 255);
                }
                else
                {
                    pixels[index] = new Color32(255, 100, 31, 255);
                }
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply(false);
    }

    private void OnDestroy()
    {
        if (waveSprite != null)
            Destroy(waveSprite);

        if (texture != null)
            Destroy(texture);
    }
}
