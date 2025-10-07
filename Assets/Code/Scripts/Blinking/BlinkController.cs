using System.Collections.Generic;
using UnityEngine;

public class BlinkController : TimedCommand
{
    [Header("Eyelid")]
    [SerializeField] List<Eyelid> _eyelids;

    [Header("Blink Timing")]
    [SerializeField] float _minStartElaspedTime;
    [SerializeField] float _maxStartElaspedTime;
    [SerializeField] float _minEndElaspedTime;
    [SerializeField] float _maxEndElaspedTime;
    [SerializeField] float _minblinkDuration, _maxBlinkDuration;
    private float _blinkDuration;
    private float _eyelidsReady;
 
    [Header("Audio")]
    [SerializeField] List<SFX> _blinkSFX = new List<SFX>();

    void OnEnable()
    {
        _minElapsedTime = _minStartElaspedTime;
        _maxElapsedTime = _maxStartElaspedTime;

        foreach (var lid in _eyelids)
            lid.BlinkComplete.AddListener(TryStartNewTimer);

        StartNewTimer();
    }

    void OnDisable()
    {
        foreach (var lid in _eyelids)
            lid.BlinkComplete.RemoveListener(TryStartNewTimer);
    }

    protected override void Execute()
    {
        //Blink
        SetTimings();
        AudioManager.Instance.PlayRandomSound(_blinkSFX);
        foreach (var lid in _eyelids)
            lid.Blink(_blinkDuration);
    }

    protected override void TryStartNewTimer()
    {
        _eyelidsReady++;
        if (_eyelidsReady == _eyelids.Count)
        {
            StartNewTimer();
            _eyelidsReady = 0;
        }
    }

    private void SetTimings()
    {
        switch (TimeTracker.Instance.DaytimeState)      //As the day progresses, the average blink time increases
        {
            case DaytimeState.Morning:
                _blinkDuration = _minblinkDuration;
                break;

            case DaytimeState.Midday:
                _blinkDuration = Random.Range(_minblinkDuration, (_maxBlinkDuration * 2) / 3f);
                break;

            case DaytimeState.Night:
                _blinkDuration = Random.Range(_maxBlinkDuration / 3f, _maxBlinkDuration);
                break;
        }

        //update for next blink
        _minElapsedTime = Mathf.Lerp(_minStartElaspedTime, _minEndElaspedTime, TimeTracker.Instance.GetCurrentNormalizedTime());    //blinking can become more frequent as the doy progresses
        _maxElapsedTime = Mathf.Lerp(_maxStartElaspedTime, _maxEndElaspedTime, TimeTracker.Instance.GetCurrentNormalizedTime());
    }
}
