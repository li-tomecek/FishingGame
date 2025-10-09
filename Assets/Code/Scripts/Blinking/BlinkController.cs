using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlinkController : TimedCommand
{
    public static BlinkController Instance;

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
    private bool _eyesClosed;
    private bool _canAutoBlink = true;

    public UnityEvent BothEyesClosed = new UnityEvent();
    private UnityAction _announceEyesClosed;

    [Header("Audio")]
    [SerializeField] List<SFX> _blinkSFX = new List<SFX>();

    void Awake()
    {
        if (Instance == null)       //Singleton
        {
            Instance = this;
            _announceEyesClosed = () => BothEyesClosed?.Invoke();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        _minElapsedTime = _minStartElaspedTime;
        _maxElapsedTime = _maxStartElaspedTime;

        foreach (var lid in _eyelids)
        {
            lid.BlinkComplete.AddListener(TryStartNewTimer);
            lid.LidClosed.AddListener(() => { _eyesClosed = true; });
        }
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

        if (TimeTracker.Instance.DaytimeState != DaytimeState.Morning)  //don't yawn in the morning.
            AudioManager.Instance.PlayRandomSound(_blinkSFX);

        foreach (var lid in _eyelids)
            lid.Blink(_blinkDuration);
    }

    protected void TryStartNewTimer()
    {
        _eyelidsReady++;
        if (_eyelidsReady == _eyelids.Count)
        {
            _eyesClosed = false;
            _eyelidsReady = 0;
            if(_canAutoBlink)
                StartNewTimer();
        }
    }

    public override IEnumerator NewTimer()      //was for debugging but leaving just in case
    {
        float timer = Random.Range(_minElapsedTime, _maxElapsedTime);
        yield return new WaitForSeconds(timer);
        _currentTimer = null;

        Execute();

        if (_automaticLoop)
            StartNewTimer();
    }

    private void SetTimings()
    {
        switch (TimeTracker.Instance.DaytimeState)      //As the day progresses, the average blink time increases
        {
            case DaytimeState.Morning:
                _blinkDuration = _minblinkDuration;
                break;

            case DaytimeState.Midday:
                _blinkDuration = Random.Range(_minblinkDuration, (_maxBlinkDuration * 2) / 3f); //min -> 2/3 of max
                break;

            case DaytimeState.Night:
                _blinkDuration = Random.Range(_maxBlinkDuration / 3f, _maxBlinkDuration);       //1/3 max -> max
                break;
        }

        //update for next blink
        _minElapsedTime = Mathf.Lerp(_minStartElaspedTime, _minEndElaspedTime, TimeTracker.Instance.GetCurrentNormalizedTime());    //blinking can become more frequent as the doy progresses
        _maxElapsedTime = Mathf.Lerp(_maxStartElaspedTime, _maxEndElaspedTime, TimeTracker.Instance.GetCurrentNormalizedTime());
    }

    public void ForceBlinkWhenReady(float duration)
    {
        StartCoroutine(WaitForForceBlink(duration, null));
    }
    
    public void ForceBlinkWhenReady(float duration, SFX customSoundEffect)
    {
        StartCoroutine(WaitForForceBlink(duration, customSoundEffect));
    }

    public IEnumerator WaitForForceBlink(float duration, SFX customSoundEffect)
    {
        _canAutoBlink = false;

        if (_currentTimer != null)          //stop any current timers
            StopCoroutine(_currentTimer);

        
        while (_eyesClosed)                 //WAIT FOR ANY ALMOST FINISHED BLINK
        {
            yield return 0;
        }

        _eyelids[0].LidClosed.AddListener(_announceEyesClosed);

        if (customSoundEffect != null)
            AudioManager.Instance.PlaySound(customSoundEffect);

        //FORCE A BLINK
        foreach (var lid in _eyelids)
            lid.Blink(duration);
        yield return new WaitForSeconds(duration);

        _eyelids[0].LidClosed.RemoveListener(_announceEyesClosed);
        BothEyesClosed.RemoveAllListeners();

        _canAutoBlink = true;
        StartNewTimer();
    }
}
