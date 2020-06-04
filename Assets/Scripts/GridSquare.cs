﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{

    public GameObject numberText;

    private int number_ = 0; //?

    private int correct_number_ = 0;

    private bool selected_ = false;

    private int square_index_ = -1;

    private bool has_default_value_ = false;

    public void SetHasDefaultValue(bool has_default) //i don't like it -> prop
    {
        has_default_value_ = has_default;
    }

    public GetHasDefaultValue() //i don't like it -> prop
    {
        return has_default_value_;
    }

    public bool IsSelected() //prop?
    {
        return selected_;
    }
    
    public void SetSquareIndex(int index) //prop?
    {
        square_index_ = index;
    }

    public void SetCorrectNumber(int number) //prop?
    {
        correct_number_ = number;
    }

    void Start()
    {
        selected_ = false;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void DisplayText()
    {
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
        number_ = number;
        DisplayText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected_ = true;
        GameEvents.SquareSelectedMethod(square_index_);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;  
        GameEvents.OnSquareSelected -= OnSquareSelected;

    }

    public void OnSetNumber(int number)
    {
        if (selected_ && !has_default_value_)
        {
            SetNumber(number);

            if (number_ != correct_number_)
            {
                var colors = this.colors; //?
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                has_default_value_ = true;
                var colors = this.colors; //?
                colors.normalColor = Color.white;
                this.colors = colors;
            }
        }
    }

    public void OnSquareSelected(int square_index)
    {
        if (square_index != square_index_)
        {
            selected_ = false;
        }
    }

}
