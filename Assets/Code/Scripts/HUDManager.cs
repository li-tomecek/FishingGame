using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{
    [SerializeField] TextMeshProUGUI _dayStateText, _timerText;
    [SerializeField] Slider _catchProgressBar;

    void Start()
    {
        TimeTracker.Instance.DaytimeStateChanged.AddListener(UpdateDayState);
        UpdateDayState(TimeTracker.Instance.DaytimeState);
    }

    void Update()
    {
        _timerText.text = $"{TimeTracker.Instance.GetCurrentTime():0} / {TimeTracker.Instance.GetLengthOfDay():0}";
    }

    void UpdateDayState(DaytimeState state)
    {
        _dayStateText.SetText(state.ToString());
    }

    public void SetCatchProgress(float progress)
    {
        _catchProgressBar.value = progress;
    }

    public void SetCatchBarActive(bool isActive)
    {
        _catchProgressBar.gameObject.SetActive(isActive);
    }
}
