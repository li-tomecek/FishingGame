using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : TimedCommand
{
    [SerializeField] GameObject _commonFishPrefab, _uncommonFishPrefab;
    [SerializeField] int _amountToPool = 5;
    [Range(0, 1)]
    [SerializeField] float _commonFishFrequency = 0.7f;

    private ObjectPool _commonFishPool;         //ideally there is one pool and we change sprites/size/values form config
    private ObjectPool _uncommonFishPool;

    void Start()
    {
        _commonFishPool = new ObjectPool(_commonFishPrefab, _amountToPool, this.gameObject);
        _uncommonFishPool = new ObjectPool(_uncommonFishPrefab, _amountToPool, this.gameObject);
        _automaticLoop = true;   //keep starting new spawn timers once one has spawned

        StartNewTimer();
    }

    protected override void Execute()
    {
        //Spawn new fish
        Fish fish = (Random.Range(0f, 1f) <= _commonFishFrequency)
        ? _commonFishPool.GetActivePooledObject()?.GetComponent<Fish>() 
        : _uncommonFishPool.GetActivePooledObject()?.GetComponent<Fish>();
 
        if (fish)
        {
            fish.gameObject.transform.position = transform.position;
            if (TimeTracker.Instance.DaytimeState == DaytimeState.Night)
                fish.SetNightSprite();
        }
      }
}
