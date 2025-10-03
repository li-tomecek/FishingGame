using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{

    [SerializeField] float _pullStrength = 15f;
    [SerializeField] float _timeToCatch = 3f;
    private float _catchTimer;
    private Coroutine _timerRoutine;

    public float PullStrength => _pullStrength;


    public void StartTimer()
    {
        _catchTimer = _timeToCatch;
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
        while (_catchTimer > 0)
        {
            yield return 0;
            _catchTimer -= Time.deltaTime;
        }

        Debug.Log("FishCaught!");
    }

}
