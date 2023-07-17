using Core.SignalBus;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSaveManager
{
    private readonly SignalBus _signalBus;

    public LevelSaveManager(SignalBus signalBus)
    {
        _signalBus = signalBus;
        Initialize();
    }

    private void Initialize()
    {
        SubscribeToSaveSignal();
    }

    private void SubscribeToSaveSignal()
    {
        _signalBus.Subscribe<SignalSaveLevel>(x => LoadNextLevel());
    }


    private void LoadNextLevel()
    {
        var nextLevelIndex =
            SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1
                ? 1
                : SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt(Constants.PlayerPrefsKey.LEVEL, nextLevelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nextLevelIndex);
    }
}