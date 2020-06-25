using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [SerializeField]
    private Text LevelText;

    [SerializeField]
    private Text TimeText;

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    void Start()
    {
        if (!Config.IsGameDataFileExist())
        {
            gameObject.GetComponent<Button>().interactable = false;
            LevelText.text = " ";
            TimeText.text = " ";
        }
        else
        {
            float deltaTime = Config.ReadGameTime();

            deltaTime += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(deltaTime);
            
            string hour = LeadingZero(span.Hours);
            string min = LeadingZero(span.Minutes);
            string sec = LeadingZero(span.Seconds);

            TimeText.text = hour + ":" + min + ":" + sec;

            LevelText.text = Config.ReadBoardLevel();
        }
    }

    public void SetGameData()
    {
        GameSettings.Instance.SetGameMode(Config.ReadBoardLevel());
    }
}
