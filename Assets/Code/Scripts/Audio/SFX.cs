using UnityEngine;

[CreateAssetMenu(fileName = "NewSFX", menuName = "Audio/SFX")]
public class SFX : ScriptableObject
{
    [field: SerializeField] public AudioClip Clip { get; private set; }
    [field: SerializeField, Range(0.0f, 1.0f)] public float Volume { get; private set; } = 1.0f;
    [field: SerializeField, Range(0.1f, 3.0f)] public float Pitch { get; private set; } = 1.0f;
}
