using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [Header("Fishing Line")]
    [SerializeField] LineRenderer _fishingLine;
    [SerializeField] Transform _endOfRod;
    [SerializeField] Transform _hook;

    void Start()
    {
        _fishingLine.positionCount = 2;
        _fishingLine.SetPosition(0, _endOfRod.position);
        _fishingLine.SetPosition(1, _hook.position);

    }

    void Update()
    {
        _fishingLine.SetPosition(0, _endOfRod.position);
    }
}
