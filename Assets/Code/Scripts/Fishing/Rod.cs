using UnityEngine;

public class FishingRod : MonoBehaviour, IButtonListener
{
    private bool _isInRange;
    [SerializeField] GameObject _arm;
    [SerializeField] Transform _hookTransform;

    [Header("Fishing Controls")]
    [SerializeField] Transform _rotationOrigin;
    [SerializeField] float _maxAngle, _minAngle, _targetAngle, _acceptedRange;

    [Header("Button Controls")]
    [SerializeField] float _pullStrength = 15f;     //Rotation of rod per second
    private float _currentPullStrength;

    [Header("Audio")]
    [SerializeField] SFX _reelSFX;
    [SerializeField] float _pitchAdjustmentWithinTarget = 1.5f; 
    [SerializeField] float _defaultPitch = 1f; 
    private AudioSource _reelSource;

    private Fish _hookedFish, _fishInRange;
    public Fish HookedFish => _hookedFish;
    public Fish FishInRange => _fishInRange;

    void Start()
    {
        FindFirstObjectByType<PlayerInputs>().RegisterListener(this);
        FishingManager.Instance.OnFishCaught.AddListener((Fish) => { ResetRod(); });
        FishingManager.Instance.OnFishLost.AddListener((Fish) => { ResetRod(); });

        _arm.transform.RotateAround(_rotationOrigin.position, Vector3.forward, (_targetAngle - _arm.transform.eulerAngles.z));
    }

    void OnDisable()
    {
        _reelSource?.Stop();
    }

    private void ResetRod()
    {
        _hookedFish = null;
        _reelSource?.Stop();
        _arm.transform.RotateAround(_rotationOrigin.position, Vector3.forward, (_targetAngle - _arm.transform.eulerAngles.z));
    }

    public void FixedUpdate()
    {
        UpdateRodAngle();
    }

    public void UpdateRodAngle()
    {
        if (_hookedFish == null) return;

        float deltaRotation = (_currentPullStrength - _hookedFish.PullStrength) * Time.fixedDeltaTime;
        deltaRotation = Mathf.Clamp(deltaRotation, _minAngle - _arm.transform.eulerAngles.z, _maxAngle - _arm.transform.eulerAngles.z);

        _arm.transform.RotateAround(_rotationOrigin.position, Vector3.forward, deltaRotation);

        if (_arm.transform.eulerAngles.z < _targetAngle - _acceptedRange || _arm.transform.eulerAngles.z > _targetAngle + _acceptedRange)
        {
            if (_isInRange) //Just left the accepted range
            {
                _hookedFish.SetTimerPause(true);
                _reelSource.pitch = _defaultPitch;
            }

            _isInRange = false;
        }
        else if (!_isInRange)   //Just re-entered the accepted range
        {
            _isInRange = true;
            _hookedFish.SetTimerPause(false);
            _reelSource.pitch = _pitchAdjustmentWithinTarget;
        }
    }

    public void SetFishInRange(Fish fish)
    {
        _fishInRange = fish;
    }
    
    #region Button actions
    public void ButtonHeld(ButtonInfo heldInfo)
    {
        //Maybe have a pull-strength multiplier that goes from 0-1 the longer you hold (04 0.5-1.5)
    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {

        if (_hookedFish)                //if in reeling state, press button to set pull strength
        {
            _currentPullStrength = _pullStrength;
        }
        else if (_fishInRange)          //if in waiting state, press button to hook valid fish
        {
            _fishInRange.Hook(_hookTransform.position);
            _hookedFish = _fishInRange;
            _fishInRange = null;

            _reelSource = AudioManager.Instance.PlayLoopingSound(_reelSFX);
        }

    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        _currentPullStrength = 0f;
    }
    #endregion
}
