﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{

    [SerializeField]
    private int colCount = 0; 
    
    [SerializeField]
    private int rowCount = 0;

    [SerializeField]
    private float squareOffset = 0.0f;

    [SerializeField]
    private GameObject gridSquare;

    [SerializeField]
    private Vector2 startPos = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private float squareScale = 1.0f;

    private List<GameObject> gridSquareList = new List<GameObject>();

    private int selectedGridData = -1;

    void Start()
    {
        if (gridSquare.GetComponent<GridSquare>() == null)
        {
            Debug.Log("This Object need to have a GridSquare script attached!");
        }
        CreateGrid();
        SetGridNumber(GameSettings.Instance.GetGameMode());
    }

    // void Update()
    // {
        
    // }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < colCount; column++)
            {   
                gridSquareList.Add(Instantiate(gridSquare) as GameObject);
                gridSquareList[gridSquareList.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                gridSquareList[gridSquareList.Count - 1].transform.parent = this.transform;
                gridSquareList[gridSquareList.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                
                square_index++;
            }
        }
    }

    private void SetSquaresPosition()
    {
        var squareRect = gridSquareList[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
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
            }
            var posXOffset = offset.x * columnNumber;
            var posYOffset = offset.y * rowNumber;
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
        for (int index = 0; index < gridSquareList.Count; index++)
        {
            gridSquareList[index].GetComponent<GridSquare>().SetNumber(data.unsolvedData[index]);
            gridSquareList[index].GetComponent<GridSquare>().SetCorrectNumber(data.solvedData[index]);
            gridSquareList[index].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolvedData[index != 0 && data.unsolvedData[index] == data.solvedData[index]]);
        }
    }
}
