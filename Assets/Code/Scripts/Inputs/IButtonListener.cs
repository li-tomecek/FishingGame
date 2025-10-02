
public interface IButtonListener
{
    public void ButtonHeld(ButtonInfo heldInfo);
    public void ButtonPressed(ButtonInfo pressedInfo);
    public void ButtonReleased(ButtonInfo releasedInfo);

}

public enum ButtonState
{
    Pressed,
    Held, 
    Released
}
public struct ButtonInfo
{
    public int ButtonID;
    public ButtonState CurrentState;
    public float TimeStarted;
    public float CurrentTime;
    public float TimeReleased;
    public float TimeHeld() => CurrentTime - TimeStarted;
}



