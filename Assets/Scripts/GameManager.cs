using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum DifferenceState
{
    Found,
    NotFound
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Changable params")]
    [SerializeField] private float _secondsForLose = 120f;
    [SerializeField] private float _tickForTimer = 1f; //in seconds
    [SerializeField] private string _levelPrefabAddress;

    [Header("Links to objects")]
    [SerializeField] private GameplayUi _gameplayUi;
    [SerializeField] private PanelFinish _panelFinish;
    [SerializeField] private GameObject _placeForLevel;

    // Fields for differences (on images)
    private Dictionary<int, List<ButtonForDifference>> _buttonsForDifference = new Dictionary<int, List<ButtonForDifference>>();
    private Dictionary<int, DifferenceState> _differencesState = new Dictionary<int, DifferenceState>();
    private int _totalCountOfDifferences = 0;
    private int _foundCountOfDifferences = 0;

    // Fields for timer
    private float _timerBeforeLose; //in seconds
    private WaitForSeconds _delayForTimer;
    private Coroutine _сoroutineTimer;

    // Fields for level
    private int _levelNumber = 0;
    private GameObject _prefabLevel;
    private GameObject _instantiatedLevel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        LoadLevel();
        _delayForTimer = new WaitForSeconds(_tickForTimer);
    }

    private void Start()
    {
        LaunchTimer();
    }

    // Method for loading level prefab as an addressable, and instantiate
    private async void LoadLevel()
    {
        // Load prefab of level by address
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(_levelPrefabAddress);
        await handle.Task;

        // Getting prefab
        _prefabLevel = handle.Result;

        InstantiateLevel();
    }

    private void InstantiateLevel()
    {
        _instantiatedLevel = Instantiate(_prefabLevel, _placeForLevel.transform);

        _levelNumber++;
        _gameplayUi.SetLevel(_levelNumber);
    }

    private void LaunchTimer()
    {
        _timerBeforeLose = _secondsForLose;

        if (_сoroutineTimer != null)
        {
            StopCoroutine(_сoroutineTimer);
        }
        _сoroutineTimer = StartCoroutine(CoruitineForTimerLose());
    }

    private IEnumerator CoruitineForTimerLose()
    {
        _gameplayUi.SetTextOfTimer(_timerBeforeLose);

        while (_timerBeforeLose > 0f)
        {
            yield return _delayForTimer;
            _timerBeforeLose -= _tickForTimer;
            _gameplayUi.SetTextOfTimer(_timerBeforeLose);
        }

        CheckForLose();
    }

    // Method save info about ButtonForDifference
    public void AddButtonForDifference(int differenceId, ButtonForDifference button)
    {
        if (!_buttonsForDifference.ContainsKey(differenceId))
        {
            _buttonsForDifference.Add(differenceId, new List<ButtonForDifference>());
        }
        _buttonsForDifference[differenceId].Add(button);

        if (!_differencesState.ContainsKey(differenceId))
        {
            // Save info about this difference id
            _differencesState.Add(differenceId, DifferenceState.NotFound);
            _totalCountOfDifferences++;
            _gameplayUi.SetTotalCountOfDifs(_totalCountOfDifferences);
        }
    }

    public void ActivateButtonsForDifference(int id)
    {
        if (!_differencesState.ContainsKey(id))
        {
            Debug.LogError($"ActivateButtonsForDifference: i have no entry for id={id}");
            return;
        }
        if (!_buttonsForDifference.ContainsKey(id))
        {
            Debug.LogError($"ActivateButtonsForDifference: i have no buttons for id={id}");
            return;
        }

        if (_differencesState[id] == DifferenceState.NotFound)
        {
            _differencesState[id] = DifferenceState.Found;
            List<ButtonForDifference> buttons = _buttonsForDifference[id];
            foreach (ButtonForDifference button in buttons)
            {
                button.DoAnimation();
            }
            _foundCountOfDifferences++;
            _gameplayUi.SetFoundCountOfDifs(_foundCountOfDifferences);

            CheckForWin();
        }
    }

    private void CheckForLose()
    {
        if (_timerBeforeLose <= 0f)
        {
            _panelFinish.ActivateWithLose();
            StopCoroutine(_сoroutineTimer);
        }
    }

    private void CheckForWin()
    {
        if (_foundCountOfDifferences == _totalCountOfDifferences)
        {
            _panelFinish.ActivateWithWin();
            StopCoroutine(_сoroutineTimer);
        }
    }

    public void Restart()
    {
        _buttonsForDifference.Clear();
        _differencesState.Clear();
        _totalCountOfDifferences = _foundCountOfDifferences = 0;
        _gameplayUi.SetFoundCountOfDifs(_foundCountOfDifferences);
        _gameplayUi.SetTotalCountOfDifs(_totalCountOfDifferences);

        Destroy(_instantiatedLevel);
        InstantiateLevel();
        LaunchTimer();
    }
}
