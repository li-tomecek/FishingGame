
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>//, ISaveable
{
    #region

    private const string PERSISTENT_SCENE_NAME = "PersistentScene";
    private const int PLAYABLE_LEVEL_INDEX = 2;
    private const int MAIN_MENU_SCENE_INDEX = 1;

    #endregion

    // --- Level Load/Unloading ---
    // ----------------------------
    #region
    public void Start()
    {
        SceneManager.sceneLoaded += SetActiveScene;
        LoadLevel(MAIN_MENU_SCENE_INDEX);
    }
    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);    //so the persistent scene is not the one being unloaded
    }

    public void LoadLevel(int buildIndex)
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(PERSISTENT_SCENE_NAME))     //do not unload the persistent scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
    }

    public void LoadMainMenu()
    {
        LoadLevel(MAIN_MENU_SCENE_INDEX);
    }

    public void LoadGameScene()
    {
        LoadLevel(PLAYABLE_LEVEL_INDEX);
    }
    #endregion
}
