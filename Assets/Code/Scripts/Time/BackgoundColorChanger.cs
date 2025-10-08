using UnityEngine;
[RequireComponent(typeof(Camera))]
public class BackgoundColorChanger : MonoBehaviour
{
    [SerializeField] Color _startColor;
    [SerializeField] Color _endColor;
    private Color _currentColor;

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.backgroundColor = _currentColor;

    }

    void Update()
    {
        _currentColor = Color.Lerp(_startColor, _endColor, TimeTracker.Instance.GetCurrentNormalizedTime());
        _camera.backgroundColor = _currentColor;
    }
}
