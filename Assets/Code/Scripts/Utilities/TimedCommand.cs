 using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class TimedCommand : MonoBehaviour
{
    [SerializeField] protected float _minElapsedTime;
    [SerializeField] protected float _maxElapsedTime;

    //protected UnityEvent _commandExecuted = new UnityEvent();
    protected bool _automaticLoop = false;

    protected abstract void Execute();

    public void StartNewTimer() { StartCoroutine(NewTimer()); }

    public IEnumerator NewTimer()
    {
        float timer = Random.Range(_minElapsedTime, _maxElapsedTime);
        yield return new WaitForSeconds(timer);

        Execute();

        if (_automaticLoop)
            StartNewTimer();
    }

    // public void StartCommandCycle()
    // {
    //     _commandExecuted.AddListener(StartNewTimer);
    //     StartNewTimer();
    // }

    // public void StopCommandCycle()
    // {
    //     StopAllCoroutines();
    //     _commandExecuted.RemoveListener(StartNewTimer);
    // }
}
