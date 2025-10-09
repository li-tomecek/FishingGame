using UnityEngine;
using UnityEngine.Events;

public class TimeTracker : Singleton<TimeTracker>
{
    [SerializeField] float _lengthOfDay = 120;
    private float _currentTime;
    private float _middayStartTime, _nightStartTime;
    private bool _dayComplete;

    public DaytimeState DaytimeState { get; private set; }
    public UnityEvent<DaytimeState> DaytimeStateChanged = new UnityEvent<DaytimeState>();
    public UnityEvent DayComplete = new UnityEvent();

    void Start()
    {
        DaytimeState = DaytimeState.Morning;
        _middayStartTime = _lengthOfDay / 3f;
        _nightStartTime = 2 * _middayStartTime;
    }
    void Update()
    {
        if (_dayComplete) return;

        _currentTime += Time.deltaTime;

        if (_currentTime >= _lengthOfDay)
        {
            DayComplete?.Invoke();
            _dayComplete = true;
            LevelManager.Instance.LoadResultsScene();
            return;
        }

        if (DaytimeState == DaytimeState.Morning && _currentTime >= _middayStartTime)
        {
            DaytimeState = DaytimeState.Midday;
            DaytimeStateChanged?.Invoke(DaytimeState);
        }
        else if (DaytimeState == DaytimeState.Midday && _currentTime >= _nightStartTime)
        {
            DaytimeState = DaytimeState.Night;
            DaytimeStateChanged?.Invoke(DaytimeState);
        }
    }

    public float GetCurrentNormalizedTime()
    {
        return _currentTime / _lengthOfDay;
    }

    public float GetCurrentTime()
    {
        return _currentTime;
    }

    public float GetLengthOfDay() { return _lengthOfDay; }

}

public enum DaytimeState
{
    Morning, Midday, Night      //Keeping it to 3 states for now, all 1/3 Of the total day time
}
