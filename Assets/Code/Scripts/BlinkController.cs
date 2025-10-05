using Unity.VisualScripting;
using UnityEngine;

public class BlinkController : TimedCommand
{
    [Header("Eyelid")]
    [SerializeField] GameObject _eyelid;
    //private float _verticalTranslation;

    [Header("Blink Timing")]
    [SerializeField] float _minStartElaspedTime;
    [SerializeField] float _maxStartElaspedTime;
    [SerializeField] float _minEndElaspedTime;
    [SerializeField] float _maxEndElaspedTime;
    [SerializeField] float _minblinkDuration, _maxBlinkDuration;
    private float _blinkDuration;

    void Start()
    {
        _minElapsedTime = _minStartElaspedTime;
        _maxElapsedTime = _maxStartElaspedTime;
        StartCommandCycle();
    }

    public override bool CanExecute()
    {
        return true;    //Don't think there are any conditions on this yet
    }

    public override void Execute()
    {
        //Blink
        SetTimings();
        Blink();
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

    private void Blink()
    {
        //bring the lid down
        //hold the lid
        //bring the lid back up
    }
}
