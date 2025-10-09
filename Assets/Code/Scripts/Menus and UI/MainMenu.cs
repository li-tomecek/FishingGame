using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startButton, _quitButton;

    public void StartGame()
    {
        LevelManager.Instance.LoadGameScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
