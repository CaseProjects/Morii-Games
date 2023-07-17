using System;
using System.Linq;
using Core.SignalBus;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Settings _settings;
    private GameObservables _gameObservables;
    private SignalBus _signalBus;
    private GameObject _currentPanel;

    private void Awake()
    {
        LoadServices();
        SubscribeToSignals();

        UpdateUI(GameStates.IDLE);
        InitializeButtons();
    }

    private void LoadServices()
    {
        _gameObservables = ServiceLocator.Get<GameObservables>();
        _signalBus = ServiceLocator.Get<SignalBus>();
    }

    private void SubscribeToSignals()
    {
        _gameObservables.GameState.Subscribe(UpdateUI);
    }

    private void InitializeButtons()
    {
        _settings.PlayButton.onClick.AddListener(() => _gameObservables.GameState.Value = GameStates.RUNNING);
        _settings.RestartButton.onClick.AddListener(() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        _settings.NextLevelButton.onClick.AddListener(() => _signalBus.Fire(new SignalSaveLevel()));
    }

    private void UpdateUI(GameStates states)
    {
        _currentPanel?.SetActive(false);
        _currentPanel = null;
        _currentPanel = GetCurrentUI(states);
        _currentPanel?.SetActive(true);
    }

    private GameObject GetCurrentUI(GameStates states)
    {
        return _settings.Panels.SingleOrDefault(x => x._states == states)?.Panel;
    }

    [Serializable]
    public class UIData
    {
        public GameStates _states;
        public GameObject Panel;
    }

    [Serializable]
    public class Settings
    {
        public UIData[] Panels;
        public Button PlayButton;
        public Button RestartButton;
        public Button NextLevelButton;
    }
}