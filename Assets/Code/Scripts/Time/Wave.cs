using UnityEngine;

public class Wave : MonoBehaviour
{
    [Header ("Vertical Movement")]
    [SerializeField] float _vertiAmplitude = 1.0f;    // Wave height
    [SerializeField] float _vertiFrequency = 1.0f;    //how many waves per second

    [Header ("Horizontal Movement")]
    [SerializeField] float _horiAmplitude = 1.0f;    // Wave height
    [SerializeField] float _horiFrequency = 1.0f;    //how many waves per second

    Vector3 _startPosition, _newPosition;
    float _timeElapsed;
    void Start()
    {
        _startPosition = transform.position;
        _timeElapsed = Random.Range(0f, 1f);    //start at a random starting point
    }

    void Update()
    {
        _timeElapsed += Time.deltaTime;
        _newPosition.y = _startPosition.y + (_vertiAmplitude * Mathf.Sin(_vertiFrequency * (_timeElapsed)));
        _newPosition.x = _startPosition.x + (_horiAmplitude * Mathf.Sin(_horiFrequency * (_timeElapsed)));

        transform.position = _newPosition;
    }

}
