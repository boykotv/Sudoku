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

    public List<GameObject> notesNumbers;
    
    private bool noteActive;
    public bool NoteActive
    {
        get
        {
            return noteActive;
        }
        set
        {
            this.noteActive = value;
        }
    }

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
            HasWrongValue = false;
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

    private bool hasWrongValue;
    public bool HasWrongValue
    {
        get
        {
            return hasWrongValue;
        }
        set
        {
            this.hasWrongValue = value;
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

    void Start()
    {
        SetNoteNumberValue(0);
    }

    public List<string> GetSquareNotes()
    {
        List<string> notes =  new List<string>();

        foreach (var number in notesNumbers)
        {
            notes.Add(number.GetComponent<Text>().text);
        }

        return notes;
    }

    private void SetClearEmptyNotes()
    {
        foreach (var number in notesNumbers)
        {
            if (number.GetComponent<Text>().text == "0")
            {
                number.GetComponent<Text>().text = " ";
            }
        }
    }

    private void SetNoteNumberValue(int value)
    {
        foreach (var number in notesNumbers)
        {
            if (value <= 0)
            {
                number.GetComponent<Text>().text = " ";
            }
            else
            {
                number.GetComponent<Text>().text = value.ToString();
            }
        }
    }

    private void SetNoteSingleNumberValue(int value, bool forceUpdate = false)
    {
        if (!NoteActive && !forceUpdate)
        {
            return;
        }        
        if (value <= 0)
        {
            notesNumbers[value - 1].GetComponent<Text>().text = " ";
        }
        else
        {
            if (notesNumbers[value - 1].GetComponent<Text>().text == " " || forceUpdate)
            {
                notesNumbers[value - 1].GetComponent<Text>().text = value.ToString();
            }
            else
            {
                notesNumbers[value - 1].GetComponent<Text>().text = " ";
            }
        }
    }

    public void SetGridNotes(List<int> notes)
    {
        foreach (var note in notes)
        {
            SetNoteSingleNumberValue(note, true);
        }
    }

    public void OnNotesActive(bool active) //dont need
    {
        NoteActive = active;
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
        GameEvents.OnNotesActive += OnNotesActive;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;  
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnNotesActive -= OnNotesActive;
    }

    public void OnSetNumber(int number)
    {
        if (IsSelected && !HasDefaultValue)
        {
            if (NoteActive && !HasWrongValue)
            {
                SetNoteSingleNumberValue(number);
            }
            else if (!NoteActive)
            {
                SetNoteNumberValue(0);
                EnteredNumber = number;
                ColorBlock colors = this.colors;

                if (EnteredNumber != CorrectNumber)
                {
                    HasWrongValue = true;

                    colors.normalColor = Color.red;
                    this.colors = colors;

                    GameEvents.OnWrongNumberMethod();
                }
                else
                {
                    HasWrongValue = false;
                    HasDefaultValue = true;

                    colors.normalColor = Color.white;
                    this.colors = colors;
                }
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

    public void SetSquareColor(Color color)
    {
        ColorBlock colors = this.colors;
        colors.normalColor = color;
        this.colors = colors;
    }
}
