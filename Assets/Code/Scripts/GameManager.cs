using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine.Events;

public class FishingManager : Singleton<FishingManager>
{
    private int _score;
    private int _fishCaught;
    public UnityEvent<int> OnScoreChange = new UnityEvent<int>();
    public UnityEvent<Fish> OnFishCaught = new UnityEvent<Fish>();  //This should be with the fish, but this way we dont have to keep adding and removing listeners
    public UnityEvent<Fish> OnFishLost = new UnityEvent<Fish>();  //This should be with the fish, but this way we dont have to keep adding and removing listeners

    void Start()
    {
        OnFishCaught.AddListener((Fish fish) => { AddToScore(fish.Value); });
    }

    public void ResetValues()
    {
        _score = 0;
        _fishCaught = 0;
    }
    public int GetFishCaught() { return _fishCaught; }
    public int GetScore() { return _score; }
    public void SetScore(int score)
    {
        _score = score;
        OnScoreChange.Invoke(score);
    }
    public void AddToScore(int toAdd)
    {
        _score += toAdd;
        _fishCaught++;
        OnScoreChange.Invoke(_score);
    }
}
