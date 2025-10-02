using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIButtonInfo : MonoBehaviour, IButtonListener
{
    [SerializeField] private TMP_Text buttonStateText, buttonInfoText;

    private ButtonInfo currentButton;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerInputs>().RegisterListener(this);
        
    }

    // Update is called once per frame

    public void ButtonHeld(ButtonInfo heldInfo) => DisplayInfo(heldInfo);
    
    public void ButtonPressed(ButtonInfo pressedInfo) => DisplayInfo(pressedInfo);

    public void ButtonReleased(ButtonInfo releasedInfo) => DisplayInfo(releasedInfo);

    public void DisplayInfo(ButtonInfo buttonInfo)
    {
        switch (buttonInfo.CurrentState)
        {
            case ButtonState.Pressed:
                buttonStateText.text = "Button Pressed";
                buttonInfoText.text = "";
                //buttonInfoText.text = $"Time since pressed {buttonInfo.T}"
                break;
            case ButtonState.Held:
                buttonStateText.text = "Button Held";
                buttonInfoText.text = $"Time held {buttonInfo.TimeHeld():0.##}";
                break;
            case ButtonState.Released:
                buttonStateText.text = "Button Released";
                buttonInfoText.text = $"Time held {buttonInfo.TimeHeld():0.##}";
                break;
        }
    }
}
