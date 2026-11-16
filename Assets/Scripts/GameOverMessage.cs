
using UnityEngine;
using TMPro;

public class GameOverMessage : MonoBehaviour
{
    public static GameOverMessage Instance { get; private set; }

    [SerializeField] private TMP_Text messageText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowLavaMessage()
    {
        SetMessage("You tried to swim in lava!");
    }

    public void ShowEnemyMessage()
    {
        SetMessage("Friendly Fire Was On!");
    }

    private void SetMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
    }
}
