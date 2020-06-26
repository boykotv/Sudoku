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

    private void OnBoardComleted()
    {
        WinPopUp.SetActive(true);
        CurrentTimeText.text = PlayTime.Instance.TextTimer.text;
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardComleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardComleted;        
    }
}
