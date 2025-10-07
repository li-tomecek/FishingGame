 using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class TimedCommand : MonoBehaviour
{
    [SerializeField] protected float _minElapsedTime;
    [SerializeField] protected float _maxElapsedTime;

    protected UnityEvent _commandExecuted = new UnityEvent();

    public abstract void Execute();
    public abstract bool CanExecute();


    public void StartNewTimer() { StartCoroutine(NewTimer()); }

    public IEnumerator NewTimer()
    {
        float timer = Random.Range(_minElapsedTime, _maxElapsedTime);
        yield return new WaitForSeconds(timer);

        if (CanExecute())
        {
            Execute();
            _commandExecuted?.Invoke();
        }
        else
        {
            StopCommandCycle();
        }
    }

    public void StartCommandCycle()
    {
        _commandExecuted.AddListener(StartNewTimer);
        StartNewTimer();
    }

    public void StopCommandCycle()
    {
        StopAllCoroutines();
        _commandExecuted.RemoveListener(StartNewTimer);
    }
}
