
using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    public static PuzzleController Instance { get; private set; }

    [SerializeField] private TMP_Text crystalsText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text bestPointsText;

    private PuzzleState puzzleState = new PuzzleState();

    private int endlessPoints;
    private int bestEndlessPoints;
    private bool endlessStarted;

    private const string BestPointsKey = "LavaRushBestCrystalPoints";

    public int CrystalCount => puzzleState.Count;
    public bool IsComplete => puzzleState.Count >= 3;
    public bool IsUnlocked => IsComplete;

    private void Awake()
    {
        Instance = this;
        bestEndlessPoints = PlayerPrefs.GetInt(BestPointsKey, 0);
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (GameFlow.Instance != null &&
            GameFlow.Instance.IsEndlessMode &&
            !endlessStarted)
        {
            StartEndless();
        }
    }

    private void StartEndless()
    {
        endlessStarted = true;
        endlessPoints = 0;
        UpdateUI();
    }

    public void CollectCrystal(int id, CrystalData data)
    {
        bool endless = GameFlow.Instance != null &&
            GameFlow.Instance.IsEndlessMode;

        if (endless)
        {
            if (!endlessStarted)
                StartEndless();

            int earned = data != null
                ? Mathf.Max(0, data.points)
                : 5;

            endlessPoints += earned;

            if (endlessPoints > bestEndlessPoints)
            {
                bestEndlessPoints = endlessPoints;
                PlayerPrefs.SetInt(BestPointsKey, bestEndlessPoints);
                PlayerPrefs.Save();
            }

            UpdateUI();
            return;
        }

        if (IsComplete)
            return;

        if (!puzzleState.Collect(id))
            return;

        UpdateUI();

        if (IsComplete)
            Debug.Log("Portal unlocked!");
    }

    public void CollectCrystal(int id)
    {
        CollectCrystal(id, null);
    }

    private void UpdateUI()
    {
        bool endless = GameFlow.Instance != null &&
            GameFlow.Instance.IsEndlessMode;

        if (crystalsText != null)
        {
            crystalsText.gameObject.SetActive(!endless);

            if (!endless)
            {
                crystalsText.text =
                    "Crystals: " + puzzleState.Count + "/3";
            }
        }

        if (statusText != null)
        {
            if (endless)
            {
                statusText.text = "ENDLESS MODE";
            }
            else if (IsComplete)
            {
                statusText.text =
                    "Portal Unlocked! Keep Climbing!";
            }
            else
            {
                statusText.text =
                    "Find " + (3 - puzzleState.Count) + " crystals";
            }
        }

        if (pointsText != null)
        {
            pointsText.gameObject.SetActive(endless);
            pointsText.text = "Crystal Points: " + endlessPoints;
        }

        if (bestPointsText != null)
        {
            bestPointsText.gameObject.SetActive(endless);
            bestPointsText.text =
                "Best Crystal Score: " + bestEndlessPoints;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
