using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

//[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ObjectShake))]
[RequireComponent(typeof(SpriteRenderer))]
public class Fish : MonoBehaviour
{
    #region Members and Properties

    [SerializeField] Transform _biteOffset;
    [SerializeField] Sprite _daySprite, _nightSprite;

    [SerializeField] int _value = 100;                  // Score
    [SerializeField] float _swimSpeed = 10f;

    [Header("Fish Pull Strength")]
    [SerializeField] float _minPullStrength = 35f;      // How strong the fish pulls on the line
    [SerializeField] float _maxPullStrength = 55f;      
    private float _pullStrength;
    const float MIN_PULL_DURATION = 1f;                 // pull strength varies, min mand max duration each time it changes 
    const float MAX_PULL_DURATION = 4f;

    [Header("Catching the Fish")]
    [SerializeField] float _timeToCatch = 3.5f;         // How long it takes to catch the fish from the starting point           
    [SerializeField] float _timerRegenRate = 0.35f;     // Regen rate of the catch timer (percent of max time regen over 1 second)          
    private float _catchTimer;
    private const float ESCAPE_SPEED = 0.5f;
    private readonly Vector3 ESCAPE_DIRECTION = new Vector3(-12, 0, 0);     //can't make a vector const

    [Header("Audio")]
    [SerializeField] private SFX _inRangeSFX;
    [SerializeField] private SFX _caughtSFX, _escapedSFX;

    private bool _hooked, _timerPaused;
    private ObjectShake _shaker;

    #endregion

    public float PullStrength => _pullStrength;
    public int Value => _value;

    public void Start()
    {
        Reset();
        _shaker = gameObject.GetComponent<ObjectShake>();
    }

    private void Reset()
    {
        _pullStrength = _maxPullStrength;
        _catchTimer = _timeToCatch;
    }

    void FixedUpdate()
    {
        if (_hooked)
        {
            if (_timerPaused)           //Catch bar drains
            {
                _catchTimer += _timerRegenRate * _timeToCatch * Time.fixedDeltaTime;
                _catchTimer = Math.Min(_catchTimer, _timeToCatch * 2f);

                if (_catchTimer >= _timeToCatch * 2f)
                    EscapeHook();
            }
            else                        //Catch bar fills
            {
                _catchTimer -= Time.fixedDeltaTime;
                if (_catchTimer <= 0)
                    Catch();
            }
            HUDManager.Instance.SetCatchProgress(1 - (_catchTimer / _timeToCatch));
        }
        else    //Fish swims forward
        {
            transform.Translate(-_swimSpeed * Time.fixedDeltaTime, 0f, 0f);     //move left on-screen by swim speed
        }     
    }

    public void SetNightSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _nightSprite;
    }

    #region Swimming
    void OnTriggerEnter2D(Collider2D other)
    {
        FishingRod rod = other.gameObject.GetComponentInParent<FishingRod>();
        if (rod == null)    //hit the despawner
        {
            SendBackToPool();
        }
        else if (rod.HookedFish == null)    //hit the hook zone
        {
            AudioManager.Instance.PlaySound(_inRangeSFX);
            rod.SetFishInRange(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        FishingRod rod = other.gameObject.GetComponentInParent<FishingRod>();
        if (rod == null) return;
        if (rod.FishInRange && rod.FishInRange == this)
            rod.SetFishInRange(null);
    }
    #endregion


    #region Catching Fish     
    public void Hook(Vector3 hookPosition)
    {
        transform.position = hookPosition - (_biteOffset.localPosition * transform.localScale.x);
        
        _hooked = true;
        HUDManager.Instance.SetCatchBarActive(true);
        StartCoroutine(VaryPullStrength());
    }

    public void SendBackToPool()
    {
        gameObject.SetActive(false);    //to send fish back to object pool
        Reset();    //Reset required values (because this object is pooled and will re-appear)
    }

    public void Catch()
    {
        _hooked = false;
        HUDManager.Instance.SetCatchBarActive(false);

        AudioManager.Instance.PlaySound(_caughtSFX);
        FishingManager.Instance.OnFishCaught.Invoke(this);

        gameObject.transform.DOMove(HUDManager.Instance.GetScoreLocation(), 0.5f).OnComplete(SendBackToPool);
    }

    public void EscapeHook()
    {
        _hooked = false;
        HUDManager.Instance.SetCatchBarActive(false);

        AudioManager.Instance.PlaySound(_escapedSFX);
        FishingManager.Instance.OnFishLost.Invoke(this);

        gameObject.transform.DOMove(ESCAPE_DIRECTION, ESCAPE_SPEED).SetRelative(true).OnComplete(SendBackToPool);
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

            _shaker.StopShake();    //just in case
            _shaker.Shake(waitTime, ((_pullStrength-_minPullStrength)/(_maxPullStrength-_minPullStrength)));    //shake vibrato depends on how hard the fish is pulling

            yield return new WaitForSeconds(waitTime);

        } while (_hooked);
    }

    #endregion

}