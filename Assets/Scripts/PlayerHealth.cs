
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float protectionTime = 2f;
    public TMP_Text healthText;
    public UnityEvent onDeath;

    private int currentHealth;
    private float protectedUntil;
    private bool dead;

    private void Start()
    {
        currentHealth = Mathf.Max(1, maxHealth);
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        if (dead || Time.time < protectedUntil)
            return;

        if (GameFlow.Instance != null &&
            GameFlow.Instance.IsEnded)
            return;

        currentHealth -= Mathf.Max(0, damage);
        currentHealth = Mathf.Max(0, currentHealth);

        UpdateHealthText();

        if (currentHealth <= 0)
        {
            dead = true;

            if (GameOverMessage.Instance != null)
            {
                GameOverMessage.Instance.ShowEnemyMessage();
            }

            onDeath?.Invoke();
        }
        else
        {
            protectedUntil = Time.time + protectionTime;
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text =
                "Health: " + currentHealth + "/" + maxHealth;
        }
    }
}
