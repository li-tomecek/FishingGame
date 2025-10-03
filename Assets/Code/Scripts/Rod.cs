using System;
using UnityEngine;
using UnityEngine.Events;

public class Rod : MonoBehaviour, IButtonListener
{
    private float _rodAngle;
    private bool _isInRange;
    [SerializeField] private Fish _hookedFish;      //Serialized for testing

    [SerializeField] Transform _rotationOrigin;
    [SerializeField] float _maxAngle, _minAngle, _targetAngle, _acceptedRange;

    [Header("Button Controls")]
    [SerializeField] float _pullStrength = 15f;     //Rotation of rod per second
    private float _currentPullStrength;

    void Start()
    {
        FindFirstObjectByType<PlayerInputs>().RegisterListener(this);
        gameObject.transform.RotateAround(_rotationOrigin.position, UnityEngine.Vector3.forward, _targetAngle);
    }

    public void FixedUpdate()
    {
        UpdateRodAngle();
    }

    public void UpdateRodAngle()
    {
        if (_hookedFish == null) return;

        float deltaRotation = (_currentPullStrength - _hookedFish.PullStrength) * Time.fixedDeltaTime;
        deltaRotation = Mathf.Clamp(deltaRotation, _minAngle - transform.eulerAngles.z, _maxAngle - transform.eulerAngles.z);

        gameObject.transform.RotateAround(_rotationOrigin.position, UnityEngine.Vector3.forward, deltaRotation);

        if (transform.eulerAngles.z < _targetAngle - _acceptedRange || transform.eulerAngles.z > _targetAngle + _acceptedRange)
        {
            if (_isInRange) //Just left the accepted range
                _hookedFish.ResetTimer();

            _isInRange = false;
        }
        else if (!_isInRange)   //Just re-entered the accepted range
        {
            _isInRange = true;
            _hookedFish.StartTimer();
                
        }

    }

    #region Button actions
    public void ButtonHeld(ButtonInfo heldInfo)
    {
        //Maybe have a pull-strength multiplier that goes from 0-1 the longer you hold (04 0.5-1.5)
    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        //if in reeling state, press button to set pull strength
        if (_hookedFish)
            _currentPullStrength = _pullStrength;

        //if in waiting state, press button to hook valid fish

    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        _currentPullStrength = 0f;
    }
    #endregion
}
