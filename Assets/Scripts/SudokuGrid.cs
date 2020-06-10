﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{

    [SerializeField]
    private int colCount; 
    
    [SerializeField]
    private int rowCount;

    [SerializeField]
    private float squareOffset;

    [SerializeField]
    private GameObject gridSquare;

    [SerializeField]
    private Vector2 startPos;

    [SerializeField]
    private float squareScale;

    [SerializeField]
    private Color LineHighlightColor;

    private List<GameObject> gridSquareList = new List<GameObject>();

    private int selectedGridData = -1;

    [SerializeField]
    private float squareDistance;

    void Start()
    {
        if (gridSquare.GetComponent<GridSquare>() == null)
        {
            Debug.Log("This Object need to have a GridSquare script attached!");
        }
        CreateGrid();
        SetGridNumber(GameSettings.Instance.GetGameMode());
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        int cellIndex = 0;

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < colCount; column++)
            {   
                gridSquareList.Add(Instantiate(gridSquare) as GameObject);
                gridSquareList[gridSquareList.Count - 1].GetComponent<GridSquare>().SquareIndex = cellIndex;
                gridSquareList[gridSquareList.Count - 1].transform.parent = this.transform;
                gridSquareList[gridSquareList.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                
                cellIndex++;
            }
        }
    }

    private void SetSquaresPosition()
    {
        var squareRect = gridSquareList[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        Vector2 squareDistanceNumb = new Vector2(0, 0);
        bool rowMoved = false;

        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + squareOffset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + squareOffset;

        int columnNumber = 0;
        int rowNumber = 0;

        foreach (GameObject square in gridSquareList)
        {
            if (columnNumber + 1 > colCount)
            {
                rowNumber++;
                columnNumber = 0;
                squareDistanceNumb.x = 0;
                rowMoved = false;
            }
            var posXOffset = offset.x * columnNumber + (squareDistanceNumb.x * squareDistance);
            var posYOffset = offset.y * rowNumber + (squareDistanceNumb.y * squareDistance);

            if (columnNumber > 0 && (columnNumber % 3 == 0))
            {
                squareDistanceNumb.x++;
                posXOffset += squareDistance;
            }

            if (rowNumber > 0 && (rowNumber % 3 == 0) && !rowMoved)  
            {
                rowMoved = true;
                squareDistanceNumb.y++;
                posYOffset += squareDistance;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + posXOffset, startPos.y - posYOffset);
            columnNumber++;
        }
    }

    private void SetGridNumber(string level)
    {
        selectedGridData = UnityEngine.Random.Range(0, SudokuData.Instance.sudokuGame[level].Count);
        var data = SudokuData.Instance.sudokuGame[level][selectedGridData];

        SetGridSquareData(data);
    }

    private void SetGridSquareData(SudokuData.SudokuBoardData data)
    {
        for (int cellIndex = 0; cellIndex < gridSquareList.Count; cellIndex++)
        {
            gridSquareList[cellIndex].GetComponent<GridSquare>().EnteredNumber = data.unsolvedData[cellIndex];
            gridSquareList[cellIndex].GetComponent<GridSquare>().CorrectNumber = data.solvedData[cellIndex];
            gridSquareList[cellIndex].GetComponent<GridSquare>().HasDefaultValue = data.unsolvedData[cellIndex] != 0 && data.unsolvedData[cellIndex] == data.solvedData[cellIndex];
        }
    }

    private void OnEnable()
    {
        GameEvents.OnSquareSelected += OnSquareSelected;
    }

    private void OnDisable() 
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
    }

    private void SetSquaresColor(int[] data, Color color)
    {
        foreach (var index in data)
        {
            
            var comp = gridSquareList[index].GetComponent<GridSquare>();
            if (!comp.HasWrongValue && !comp.IsSelected)
            {
                comp.SetSquareColor(color);
            }
        }
    }

    public void OnSquareSelected(int squareIndex)
    {
        var horizontalLine = LineIndicator.instance.GetHorizontalLine(squareIndex);
        var verticalLine = LineIndicator.instance.GetVerticalLine(squareIndex);
        var square = LineIndicator.instance.GetSquare(squareIndex);

        SetSquaresColor(LineIndicator.instance.GetAllSquaresIndexes(), Color.white);
        
        SetSquaresColor(horizontalLine, LineHighlightColor);
        SetSquaresColor(verticalLine, LineHighlightColor);
        SetSquaresColor(square, LineHighlightColor);
    }
}
