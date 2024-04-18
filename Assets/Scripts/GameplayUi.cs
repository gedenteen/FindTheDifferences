using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private TextMeshProUGUI textTotalCountOfDifs;
    [SerializeField] private TextMeshProUGUI textFoundCountOfDifs;
    [SerializeField] private TextMeshProUGUI textTimer;

    public void SetLevel(int level)
    {
        textLevel.text = level.ToString();
    }

    public void SetTextOfTimer(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        textTimer.text = formattedTime;
    }

    public void SetTotalCountOfDifs(int count)
    {
        textTotalCountOfDifs.text = "/ " + count.ToString();
    }

    public void SetFoundCountOfDifs(int count)
    {
        textFoundCountOfDifs.text = count.ToString();
    }
}
