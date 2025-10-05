using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fish : MonoBehaviour
{

    [Header("Catching the Fish")]
    [SerializeField] float _pullStrength = 15f;     // How strong the fish pulls on the line
    [SerializeField] float _timeToCatch = 3f;       // How long it takes to catch the fish
    [SerializeField] int _value = 100;              // For the score
    [SerializeField] LayerMask _hookLayer;

    [Header("Swimming")]
    [SerializeField] float _swimSpeed = 10f;

    private bool _hooked;
    private Coroutine _timerRoutine;
    public float PullStrength => _pullStrength;

    #region Swimming
    void FixedUpdate()
    {
        if (_hooked) return;

        transform.Translate(-_swimSpeed * Time.fixedDeltaTime, 0f, 0f);     //move left on-screen by swim speed
    }

    void OnTriggerEnter2D(Collider2D other)     //This should maybe be moved to the rod?
    {
        _hooked = true;
        //TODO: play visual/audio cues here!
        other.gameObject.GetComponentInParent<FishingRod>().HookFish(this);
    }
    #endregion


    #region Catching Fish
    public void StartTimer()
    {
        _timerRoutine = StartCoroutine(CatchTimer());
        Debug.Log("timer started");
    }

    public void ResetTimer()
    {
        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);

        Debug.Log("timer stopped");
    }

    public IEnumerator CatchTimer()
    {
        yield return new WaitForSeconds(_timeToCatch);

        Debug.Log("FishCaught!");
    }

    public void ReleaseFish()
    {
        _hooked = false;
        gameObject.SetActive(false);    //to send fish back to object pool
    }

    #endregion

}
