using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : TimedCommand
{
    [SerializeField] GameObject _fishPrefab;
    [SerializeField] int _amountToPool = 5;

    private ObjectPool _fishPool;

    void Start()
    {
        _fishPool = new ObjectPool(_fishPrefab, _amountToPool, this.gameObject);
        StartCommandCycle();
    }


    public override bool CanExecute()
    {
        //If the player is not currently reeling in another fish
        return true;    //TEMP
    }

    public override void Execute()
    {
        //Spawn new fish
        Fish fish = _fishPool.GetActivePooledObject()?.GetComponent<Fish>();
        if(fish)
            fish.gameObject.transform.position = transform.position;
    }
}
