using UnityEngine;
using System.Collections.Generic;

namespace RacingGame.Core
{
    /// <summary>
    /// Central game manager handling core game state and lifecycle.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        [SerializeField] private bool _dontDestroyOnLoad = true;

        private GameState _currentState = GameState.MainMenu;
        private Dictionary<GameState, IGameStateHandler> _stateHandlers = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            InitializeStateHandlers();
        }

        private void InitializeStateHandlers()
        {
            _stateHandlers[GameState.MainMenu] = new MainMenuStateHandler();
            _stateHandlers[GameState.Gameplay] = new GameplayStateHandler();
            _stateHandlers[GameState.Paused] = new PausedStateHandler();
            _stateHandlers[GameState.RaceActive] = new RaceActiveStateHandler();
        }

        public void ChangeGameState(GameState newState)
        {
            if (_currentState == newState)
                return;

            if (_stateHandlers.TryGetValue(_currentState, out var exitHandler))
            {
                exitHandler.OnExit();
            }

            _currentState = newState;

            if (_stateHandlers.TryGetValue(_currentState, out var enterHandler))
            {
                enterHandler.OnEnter();
            }
        }

        public GameState GetCurrentState() => _currentState;

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }

    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        RaceActive,
        Garage,
        Results
    }

    public interface IGameStateHandler
    {
        void OnEnter();
        void OnExit();
    }

    public class MainMenuStateHandler : IGameStateHandler
    {
        public void OnEnter() => Debug.Log("Entering Main Menu");
        public void OnExit() => Debug.Log("Exiting Main Menu");
    }

    public class GameplayStateHandler : IGameStateHandler
    {
        public void OnEnter() => Debug.Log("Entering Gameplay");
        public void OnExit() => Debug.Log("Exiting Gameplay");
    }

    public class PausedStateHandler : IGameStateHandler
    {
        public void OnEnter() => Debug.Log("Game Paused");
        public void OnExit() => Debug.Log("Game Resumed");
    }

    public class RaceActiveStateHandler : IGameStateHandler
    {
        public void OnEnter() => Debug.Log("Race Started");
        public void OnExit() => Debug.Log("Race Ended");
    }
}
