using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : TimedCommand
{
    [SerializeField] GameObject _fishPrefab;
    [SerializeField] int _amountToPool = 5;

    private ObjectPool _fishPool;
    private bool _useNightSprites;

    void Start()
    {
        _fishPool = new ObjectPool(_fishPrefab, _amountToPool, this.gameObject);
        _automaticLoop = true;   //keep starting new spawn timers once one has spawned

        StartNewTimer();
    }

    protected override void Execute()
    {
        //Spawn new fish
        Fish fish = _fishPool.GetActivePooledObject()?.GetComponent<Fish>();
        if(fish)
            fish.gameObject.transform.position = transform.position;
    }
}
