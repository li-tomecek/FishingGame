using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField] TextMeshProUGUI _dayStateText, _timerText, _scoreText;
    [SerializeField] Slider _catchProgressBar;

    void Start()
    {
        TimeTracker.Instance.DaytimeStateChanged.AddListener(UpdateDayState);
        UpdateDayState(TimeTracker.Instance.DaytimeState);
        FishingManager.Instance.OnScoreChange.AddListener(UpdateScore);
    }

    // --- DAY CYCLE ---
    // -----------------
    void Update()   //TEMP
    {
        _timerText.text = $"{TimeTracker.Instance.GetCurrentTime():0} / {TimeTracker.Instance.GetLengthOfDay():0}";
    }

    void UpdateDayState(DaytimeState state)
    {
        _dayStateText.SetText(state.ToString());
    }

    // --- CATCH PROGRESS BAR ---
    // --------------------------
    public void SetCatchProgress(float progress)
    {
        _catchProgressBar.value = progress;
    }

    public void SetCatchBarActive(bool isActive)
    {
        _catchProgressBar.gameObject.SetActive(isActive);
    }

    // --- SCORE ---
    // -------------
    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public Vector3 GetScoreLocation()
    {
        return _scoreText.transform.position;
    }
}
