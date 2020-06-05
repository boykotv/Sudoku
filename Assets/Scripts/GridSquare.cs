using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject squareText;

    private int enteredNumber = 0;
    public int EnteredNumber
    {
        get
        {
            return enteredNumber;
        }
        set
        {
            this.enteredNumber = value;
            DisplayText();
        }
    }

    private int correctNumber;
    public int CorrectNumber
    {
        get
        {
            return correctNumber;
        }
        set
        {
            this.correctNumber = value;
        }
    }

    private bool isSelected;
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            this.isSelected = value;
        }
    }

    private bool hasDefaultValue;
    public bool HasDefaultValue
    {
        get
        {
            return hasDefaultValue;
        }
        set
        {
            this.hasDefaultValue = value;
        }
    }

    private int squareIndex;
    public int SquareIndex
    {
        get
        {
            return squareIndex;
        }
        set
        {
            this.squareIndex = value;
        }
    }

    public void DisplayText()
    {
        squareText.GetComponent<Text>().text = EnteredNumber <= 0 ? " " : EnteredNumber.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsSelected = true;
        GameEvents.SquareSelectedMethod(SquareIndex);
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
        if (IsSelected && !HasDefaultValue)
        {
            EnteredNumber = number;
            ColorBlock colors = this.colors;

            if (EnteredNumber != CorrectNumber)
            {
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                HasDefaultValue = true;
                colors.normalColor = Color.white;
                this.colors = colors;
            }
        }
    }

    public void OnSquareSelected(int square_index)
    {
        if (square_index != SquareIndex)
        {
            IsSelected = false;
        }
    }

}
