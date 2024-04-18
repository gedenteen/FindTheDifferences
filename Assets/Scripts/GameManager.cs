using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

// [System.Serializable]
// public class LinksToButtonsForDifference
// {
//     public int id;
//     public List<ButtonForDifference> buttons = new List<ButtonForDifference>();
// }

public enum DifferenceState
{
    Found,
    NotFound
}

public class GameManager : MonoBehaviour
{
    [Header("Changable params")]
    [SerializeField] private float _secondsForLose = 120f;
    [SerializeField] private float _tickForTimer = 1f; //in seconds


    [Header("Links to objects")]
    [SerializeField] private GameplayUi _gameplayUi;
    [SerializeField] private PanelFinish _panelFinish;

    // Fields for differences (on images)
    private Dictionary<int, List<ButtonForDifference>> _buttonsForDifference = new Dictionary<int, List<ButtonForDifference>>();
    private Dictionary<int, DifferenceState> _differencesState = new Dictionary<int, DifferenceState>();
    private int _totalCountOfDifferences = 0;
    private int _foundCountOfDifferences = 0;

    // Fields for timer
    private float _timerBeforeLose; //in seconds
    private WaitForSeconds _delayForTimer;

    private void Start()
    {
        _timerBeforeLose = _secondsForLose;
        _delayForTimer = new WaitForSeconds(_tickForTimer);
        StartCoroutine(CoruitineForTimerLose());
    }

    private IEnumerator CoruitineForTimerLose()
    {
        while (_timerBeforeLose > 0f)
        {
            _gameplayUi.SetTextOfTimer(_timerBeforeLose);
            yield return _delayForTimer;
            _timerBeforeLose -= _tickForTimer;
        }

        if (_timerBeforeLose <= 0f)
        {
            _panelFinish.ActivateWithLose();
        }
    }

    public void AddButtonForDifference(int buttonId, ButtonForDifference button)
    {
        if (!_buttonsForDifference.ContainsKey(buttonId))
        {
            _buttonsForDifference.Add(buttonId, new List<ButtonForDifference>());
        }
        _buttonsForDifference[buttonId].Add(button);

        if (!_differencesState.ContainsKey(buttonId))
        {
            _differencesState.Add(buttonId, DifferenceState.NotFound);
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

    private void CheckForWin()
    {
        if (_foundCountOfDifferences == _totalCountOfDifferences)
        {
            _panelFinish.ActivateWithWin();
        }
    }
}
