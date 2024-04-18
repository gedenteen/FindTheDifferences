using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonForDifference : MonoBehaviour
{
    [Header("Changable params")]
    [SerializeField] private int myId;

    [Header("Links to objects")]
    [SerializeField] private Button button;
    [SerializeField] private Image imageWithCheckMark;

    private void Awake()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.AddButtonForDifference(myId, this);

        button.onClick.AddListener(ActionForClick);
    }

    private void ActionForClick()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.ActivateButtonsForDifference(myId);
    }

    public void DoAnimation()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        float fillAmount = 0f;
        while (fillAmount < 1f)
        {
            fillAmount += 0.02f;
            imageWithCheckMark.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.02f);
        }
    }

}
