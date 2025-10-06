using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dayStateText, _timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
