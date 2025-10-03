using System;
using UnityEngine;
using UnityEngine.Events;

public class Rod : MonoBehaviour, IButtonListener
{
    private float _rodAngle;
    private bool _fishOnHook;
    [SerializeField] private Fish _hookedFish;      //Serialized for testing

    [SerializeField] Transform _rotationOrigin;
    [SerializeField] float _maxAngle, _minAngle, _targetAngle, _acceptedRange;

    [Header("Button Controls")]
    [SerializeField] float _pullStrength = 15f;     //Rotation of rod per second
    private float _currentPullStrength;

    private UnityEvent _enteredTargetRange;
    private UnityEvent _exitTargetRange;

    void Start()
    {
        FindFirstObjectByType<PlayerInputs>().RegisterListener(this);

        _enteredTargetRange = new UnityEvent();
        _exitTargetRange = new UnityEvent();

        gameObject.transform.RotateAround(_rotationOrigin.position, UnityEngine.Vector3.forward, _targetAngle);

        _fishOnHook = true; //for testing
    }

    public void FixedUpdate()
    {
        UpdateRodAngle();
    }

    public void UpdateRodAngle()
    {
        float deltaRotation = (_currentPullStrength - _hookedFish.PullStrength) * Time.fixedDeltaTime;
        deltaRotation = Mathf.Clamp(deltaRotation, _minAngle - transform.eulerAngles.z, _maxAngle - transform.eulerAngles.z);

        gameObject.transform.RotateAround(_rotationOrigin.position, UnityEngine.Vector3.forward, deltaRotation);
    }

    #region Button actions
    public void ButtonHeld(ButtonInfo heldInfo)
    {

    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        //if in reeling state, press button to set pull strength
        if (_fishOnHook)
            _currentPullStrength = _pullStrength;

        //if in waiting state, press button to hook valid fish

    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        _currentPullStrength = 0f;
    }
    #endregion
}
