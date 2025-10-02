using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// JumpBall implements IButtonlistener, an interface, which means that it has to implement those functions
/// </summary>
public class JumpBall : MonoBehaviour, IButtonListener
{
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float maxHeight = 5f;
    [SerializeField] private float jumpSpeedIncrease = 1f;
    private Rigidbody2D _rb;
    private ButtonInfo _currentButton;
    
    private PlayerInputs inputObject;
    
    void Awake()
    {
        _currentButton.CurrentState = ButtonState.Released;
        _rb = GetComponent<Rigidbody2D>();
        var inputObject = FindObjectOfType<PlayerInputs>();
        inputObject.RegisterListener(this);
        _rb.linearVelocity = new Vector2(0, -jumpForce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_currentButton.CurrentState == ButtonState.Released)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y - jumpSpeedIncrease * Time.fixedDeltaTime);
        }
    }

    public void ButtonHeld(ButtonInfo heldInfo)
    {
        if(this.transform.position.y < maxHeight)
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y + jumpSpeedIncrease * Time.fixedDeltaTime);
        else 
            _rb.linearVelocity = new Vector2(0, 0);
        
    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y + jumpForce);
        _currentButton = pressedInfo;
    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        _rb.linearVelocity = new Vector2(0, -jumpForce);
        _currentButton = releasedInfo;
    }
}
