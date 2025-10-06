using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Fish : MonoBehaviour
{
    [SerializeField] int _value = 100;                  // Score

    [Header("Fish Pull Strength")]
    [SerializeField] float _minPullStrength = 35f;      // How strong the fish pulls on the line
    [SerializeField] float _maxPullStrength = 55f;      // How strong the fish pulls on the line
    private float _pullStrength;
    const float MIN_PULL_DURATION = 1f;                 // pull strength varies, min mand max duration of each variance () 
    const float MAX_PULL_DURATION = 4f;

    [Header("Catching the Fish")]
    [SerializeField] float _timeToCatch = 5f;           // How long it takes to catch the fish           
    [SerializeField] float _timerRegenRate = 0.2f;      // Regen rate of the catch timer (percent of max time regen over 1 second)          
    private float _catchTimer;

    [Header("Swimming")]
    [SerializeField] float _swimSpeed = 10f;

    private bool _hooked, _timerPaused;
    public float PullStrength => _pullStrength;
    public int Value => _value;

    public void Start()
    {
        _pullStrength = _maxPullStrength;
        _catchTimer = _timeToCatch;
    }

    void FixedUpdate()
    {
        if (_hooked)
        {
            if (_timerPaused)
            {
                _catchTimer += _timerRegenRate * _timeToCatch * Time.fixedDeltaTime;
                _catchTimer = Math.Min(_catchTimer, _timeToCatch);
            }
            else
            {
                _catchTimer -= Time.fixedDeltaTime;
                if (_catchTimer <= 0)
                    Catch();
            }
            HUDManager.Instance.SetCatchProgress(1 - (_catchTimer / _timeToCatch));
        }
        else
        {
            transform.Translate(-_swimSpeed * Time.fixedDeltaTime, 0f, 0f);     //move left on-screen by swim speed
        }     
    }

    #region Swimming
    void OnTriggerEnter2D(Collider2D other)
    {
        //TODO: play visual/audio cues here!
        FishingRod rod = other.gameObject.GetComponentInParent<FishingRod>();
        if (rod.HookedFish == null && rod.FishInRange == null)
            rod.SetFishInRange(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        FishingRod rod = other.gameObject.GetComponentInParent<FishingRod>();
        if (rod.FishInRange && rod.FishInRange == this)
            rod.SetFishInRange(null);
    }
    #endregion


    #region Catching Fish     
    public void Hook()      //should maybe have an event for this ni rod side
    {
        _hooked = true;
        HUDManager.Instance.SetCatchBarActive(true);
        StartCoroutine(VaryPullStrength());
    }

    public void Release()
    {
        _hooked = false;
        HUDManager.Instance.SetCatchBarActive(false);
        gameObject.SetActive(false);    //to send fish back to object pool

        Start();    //Reset required values (because this object is pooled and will reappear)
    }

    public void Catch()
    {
        Debug.Log("FISH CAUGHT");
        FishingManager.Instance.OnFishCaught.Invoke(this);
        Release();  //temp
    }

    public void SetTimerPause(bool paused)
    {
        _timerPaused = paused;
    }

    public IEnumerator VaryPullStrength()
    {
        float waitTime;
        do
        {
            _pullStrength = UnityEngine.Random.Range(_minPullStrength, _maxPullStrength);
            waitTime = UnityEngine.Random.Range(MIN_PULL_DURATION, MAX_PULL_DURATION);
            //Debug.Log($"Pull Strength is {_pullStrength} for {waitTime} seconds");
            yield return new WaitForSeconds(waitTime);

        } while (_hooked);
    }

    #endregion

}


    //OLD CATCH TIMER  (just in case)

    // public void StartTimer()
    // {
    //     _timerRoutine = StartCoroutine(CatchTimer());
    //     Debug.Log("timer started");
    // }

    // public void ResetTimer()
    // {
    //     if (_timerRoutine != null)
    //         StopCoroutine(_timerRoutine);

    //     Debug.Log("timer stopped");
    // }

    // public IEnumerator CatchTimer()
    // {
    //     yield return new WaitForSeconds(_timeToCatch);
    //     _hooked = false;
    //     StopAllCoroutines();    //to stop pull variance
    //     Debug.Log("FishCaught!");
    // }
