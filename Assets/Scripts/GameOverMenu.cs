using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private Text CurrentTimeText;
    
    void Start()
    {
        CurrentTimeText.text = PlayTime.instance.TextTimer.text;
    }

}
