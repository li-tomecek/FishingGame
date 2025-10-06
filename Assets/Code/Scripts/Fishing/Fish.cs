using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fish : MonoBehaviour
{
    [SerializeField] int _value = 100;                  // Score

    [Header("Catching the Fish")]
    [SerializeField] float _minPullStrength = 35f;      // How strong the fish pulls on the line
    [SerializeField] float _maxPullStrength = 55f;      // How strong the fish pulls on the line
    [SerializeField] float _timeToCatch = 5f;           // How long it takes to catch the fish                 // For the score
    [SerializeField] LayerMask _hookLayer;
    private float _pullStrength;

    const float MIN_PULL_DURATION = 1f;                 // pull strength varies, min mand max duration of each variance () 
    const float MAX_PULL_DURATION = 4f;

    [Header("Swimming")]
    [SerializeField] float _swimSpeed = 10f;

    private bool _hooked, _timerPaused;
    private Coroutine _timerRoutine;
    public float PullStrength => _pullStrength;

    public void Start()
    {
        _pullStrength = _maxPullStrength;
    }

    #region Swimming
    void FixedUpdate()
    {
        if (_hooked)
        {
            if (_timerPaused)
                return;

            _timeToCatch -= Time.fixedDeltaTime;
            if (_timeToCatch <= 0)
                CatchFish();
        }
        else
        {
            transform.Translate(-_swimSpeed * Time.fixedDeltaTime, 0f, 0f);     //move left on-screen by swim speed
        }     
    }

    void OnTriggerEnter2D(Collider2D other)     //This should maybe be moved to the rod?
    {
        _hooked = true;
        //TODO: play visual/audio cues here!
        other.gameObject.GetComponentInParent<FishingRod>().HookFish(this);
        StartCoroutine(VaryPullStrength());
    }
    #endregion


    #region Catching Fish       //Note: none of this is currently being used anymore
    public void SetTimerPause(bool paused)
    {
        _timerPaused = paused;
    }

    public void ReleaseFish()
    {
        _hooked = false;
        gameObject.SetActive(false);    //to send fish back to object pool
    }

    public void CatchFish()
    {
        Debug.Log("FISH CAUGHT");
        ReleaseFish();  //temp
    }

    public IEnumerator VaryPullStrength()
    {
        float waitTime;
        do
        {
            _pullStrength = Random.Range(_minPullStrength, _maxPullStrength);
            waitTime = Random.Range(MIN_PULL_DURATION, MAX_PULL_DURATION);
            Debug.Log($"Pull Strenght is {_pullStrength} for {waitTime} seconds");
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
