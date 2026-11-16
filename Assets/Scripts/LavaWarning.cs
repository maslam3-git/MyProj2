
using System.Collections;
using UnityEngine;
using TMPro;

public class LavaWarning : MonoBehaviour
{
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private float displayTime = 3f;

    private void Awake()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

    private IEnumerator Start()
    {
        if (warningText == null)
        {
            Debug.LogError("LavaWarning needs a warning text reference.");
            yield break;
        }

        LavaController lava = FindFirstObjectByType<LavaController>();

        if (lava == null)
        {
            Debug.LogError("LavaController was not found.");
            yield break;
        }

        yield return new WaitForSeconds(lava.StartDelay);

        if (GameFlow.Instance != null && GameFlow.Instance.IsEnded)
            yield break;

        warningText.gameObject.SetActive(true);

        float timer = 0f;

        while (timer < displayTime)
        {
            if (GameFlow.Instance != null && GameFlow.Instance.IsEnded)
            {
                warningText.gameObject.SetActive(false);
                yield break;
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        warningText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameFlow.Instance != null &&
            GameFlow.Instance.IsEnded &&
            warningText != null &&
            warningText.gameObject.activeSelf)
        {
            warningText.gameObject.SetActive(false);
        }
    }
}
