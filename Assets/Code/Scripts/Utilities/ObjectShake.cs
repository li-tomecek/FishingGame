using DG.Tweening;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [SerializeField] float _defaultDuration;
    [SerializeField] float _defaultStrength = 1f;
    [SerializeField][Range(0f, 180f)] float _defaultRandomness = 90f;
    [SerializeField] int _defaultVibrato = 10;

    Tween _tween;

    public void Shake()
    {
        _tween = gameObject.transform.DOShakePosition(_defaultDuration, _defaultStrength, _defaultVibrato, _defaultRandomness, fadeOut:false, randomnessMode: ShakeRandomnessMode.Harmonic);
    }

    public void Shake(float duration, int vibrato)
    {
        _tween = gameObject.transform.DOShakePosition(duration, _defaultStrength, vibrato, _defaultRandomness, fadeOut:false, randomnessMode: ShakeRandomnessMode.Harmonic);
    }

    public void StopShake()
    {
        if (_tween != null)
        {
            _tween.Kill();
        }
    }
}
