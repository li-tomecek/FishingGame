using UnityEngine;

public class NightChanges : MonoBehaviour
{
    [SerializeField] GameObject _oceanDay, _oceanNight;

    void Start()
    {
        TimeTracker.Instance.DaytimeStateChanged.AddListener(RegisterNightBlinkListener);
    }

    private void RegisterNightBlinkListener(DaytimeState dayState)
    {
        if (dayState == DaytimeState.Night)
        {
            //change ocean colors
            BlinkController.Instance.BothEyesClosed.AddListener(ChangeBackground);
            BlinkController.Instance.ForceBlinkWhenReady(3.5f);    //eyes closed for over a second
        }
    }

    private void ChangeBackground()
    {
        BlinkController.Instance.BothEyesClosed.RemoveListener(ChangeBackground);
        _oceanDay.SetActive(false);
        _oceanNight.SetActive(true);
    }
}
