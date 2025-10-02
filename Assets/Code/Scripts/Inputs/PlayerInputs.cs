using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Just an example of a player input class for collecting button info. Place this on an object in your scene, then subscribe to it in order
/// </summary>
public class PlayerInputs : MonoBehaviour
{
    private GameInputs _gameInputs;
    private ButtonInfo _currentButton;
    private UnityEvent<ButtonInfo> _buttonHeld;
    private UnityEvent<ButtonInfo> _buttonReleased;
    private UnityEvent<ButtonInfo> _buttonPressed;
    public List<ButtonInfo> _buttonInputs = new List<ButtonInfo>();
    private Coroutine _buttonHeldRoutine;
    public void Awake()
    {
        _buttonHeld = new UnityEvent<ButtonInfo>();
        _buttonReleased = new UnityEvent<ButtonInfo>();
        _buttonPressed = new UnityEvent<ButtonInfo>();
        
    }
    public void OnEnable()
    {
        if (_gameInputs == null)
        {
            _gameInputs = new GameInputs();
            _gameInputs.PlayerControl.Interact.performed += (val) =>
            {
                _currentButton = new ButtonInfo()
                    { ButtonID = _buttonInputs.Count, TimeStarted = Time.time, CurrentTime = Time.time, CurrentState= ButtonState.Pressed};
                _buttonInputs.Add(_currentButton);
                _buttonPressed.Invoke(new ButtonInfo());
                StartCoroutine(ButtonHeld());
                
            };
            _gameInputs.PlayerControl.Interact.canceled += (val) =>
            {
                StopAllCoroutines();
                _currentButton.CurrentTime = Time.time;
                _currentButton.TimeReleased = Time.time;
                _currentButton.CurrentState = ButtonState.Released;
                _buttonReleased.Invoke(_currentButton);
            };
            
        }
        _gameInputs.PlayerControl.Enable();
    }

    private void OnDestroy()
    {
        _gameInputs?.Disable();
    }

    public void RegisterListener(IButtonListener buttonListener)
    {
        _buttonPressed.AddListener(buttonListener.ButtonPressed);
        _buttonHeld.AddListener(buttonListener.ButtonHeld);
        _buttonReleased.AddListener(buttonListener.ButtonReleased);
    }
    public void DeRegisterListener(IButtonListener buttonListener)
    {
        _buttonPressed.RemoveListener(buttonListener.ButtonPressed);
        _buttonHeld.RemoveListener(buttonListener.ButtonHeld);
        _buttonReleased.RemoveListener(buttonListener.ButtonReleased);
    }
    IEnumerator ButtonHeld()
    {
        while (true && _gameInputs != null)
        {
            _currentButton.CurrentState = ButtonState.Held;
            _currentButton.CurrentTime = Time.time;
            _buttonHeld.Invoke(_currentButton);
            yield return null;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

