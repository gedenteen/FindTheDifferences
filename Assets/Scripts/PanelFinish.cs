using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelFinish : MonoBehaviour
{
    [Header("Links to objects")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _textMain;

    public void ActivateWithWin()
    {
        _canvasGroup.gameObject.SetActive(true);
        _textMain.text = "You win";
    }

    public void ActivateWithLose()
    {
        _canvasGroup.gameObject.SetActive(true);
        _textMain.text = "You lose";
    }

    public void Restart()
    {
        GameManager.instance.Restart();
        _canvasGroup.gameObject.SetActive(false);
    }
}
