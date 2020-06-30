using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    [SerializeField]
    private GameObject WinPopUp;

    [SerializeField]
    private Text CurrentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        WinPopUp.SetActive(false);
    }

    private void OnBoardCompleted()
    {
        WinPopUp.SetActive(true);
        CurrentTimeText.text = PlayTime.Instance.TextTimer.text;
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted;        
    }
}
