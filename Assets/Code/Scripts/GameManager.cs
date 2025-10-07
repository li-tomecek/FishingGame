using System.Diagnostics;
using UnityEngine.Events;

public class FishingManager : Singleton<FishingManager>
{
    private int _score;
    public UnityEvent<int> OnScoreChange = new UnityEvent<int>();
    public UnityEvent<Fish> OnFishCaught = new UnityEvent<Fish>();  //This should be with the fish, but this way we dont have to keep adding and removing listeners

    void Start()
    {
        OnFishCaught.AddListener((Fish fish) => { AddToScore(fish.Value); });
    }

    public int GetScore() { return _score; }
    public void SetScore(int score)
    {
        _score = score;
        OnScoreChange.Invoke(score);
    }
    public void AddToScore(int toAdd)
    {
        Debug.WriteLine("Adding to score??");
        _score += toAdd;
        OnScoreChange.Invoke(_score);
    }
}
