using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Eyelid : MonoBehaviour
{
    private float _startY;
    
    public UnityEvent BlinkComplete = new UnityEvent();
    public UnityEvent LidClosed = new UnityEvent();

    void Start()
    {
        _startY = gameObject.transform.position.y;
    }

    public void Blink(float blinkDuration)
    {
        //Debug.Log($"playing with duration: {blinkDuration}");
        StartCoroutine(BlinkRoutine(blinkDuration));
    }

    public IEnumerator BlinkRoutine(float blinkDuration)
    {
        yield return gameObject.transform.DOMoveY(0f, blinkDuration / 3f);
        LidClosed?.Invoke();
        yield return new WaitForSeconds(blinkDuration / 3f);
        yield return gameObject.transform.DOMoveY(_startY, blinkDuration / 3f);
        BlinkComplete?.Invoke();
    }
    
}
