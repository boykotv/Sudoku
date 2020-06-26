using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Text CurrentTimeText;

   public void DisplayTime()
   {
        CurrentTimeText.text = PlayTime.Instance.TextTimer.text;
   }
}
