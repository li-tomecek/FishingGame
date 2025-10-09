using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startButton, _quitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void StartGame()
    {
        LevelManager.Instance.LoadGameScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
