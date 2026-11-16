
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    [SerializeField] private ParticleSystem burstPrefab;

    private int crystalID;
    private CrystalData crystalData;
    private bool collected;

    public void Configure(int id, CrystalData data)
    {
        crystalID = id;
        crystalData = data;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null && crystalData != null)
            sprite.color = crystalData.crystalColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider2D other)
    {
        if (collected)
            return;

        if (other.GetComponentInParent<PlayerController>() == null)
            return;

        if (PuzzleController.Instance == null)
            return;

        if (GameFlow.Instance != null &&
            GameFlow.Instance.IsEnded)
            return;

        collected = true;

        PuzzleController.Instance.CollectCrystal(
            crystalID,
            crystalData
        );

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCrystal();

        if (burstPrefab != null)
        {
            ParticleSystem burst = Instantiate(
                burstPrefab,
                transform.position,
                Quaternion.identity
            );

            burst.Play();
            Destroy(burst.gameObject, 3f);
        }

        Destroy(gameObject);
    }
}
