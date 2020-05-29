﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : Selectable
{

    public GameObject numberText;

    private int number_ = 0; //?

    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void DisplayText()
    {
        Debug.Log("DisplayText Start");
        if (number_ <= 0)
        {
            numberText.GetComponent<Text>().text = " ";
        }
        else
        {
            numberText.GetComponent<Text>().text = number_.ToString();
        }
    }

    public void SetNumber(int number)
    {
        Debug.Log("SetNumber Start");
        number_ = number;
        DisplayText();
    }

}