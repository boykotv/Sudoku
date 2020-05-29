using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{

    public int columns = 0; //colCount
    
    public int rows = 0; //rowCount

    public float squareOffset = 0.0f;

    public GameObject gridSquare;

    public Vector2 startPos = new Vector2(0.0f, 0.0f);

    public float squareScale = 1.0f;

    private List<GameObject> gridSquareList = new List<GameObject>();

    void Start()
    {
        Debug.Log("SudokuGrid Start");
        if (gridSquare.GetComponent<GridSquare>() == null)
        {
            Debug.Log("This Object need to have a GridSquare script attached!");
        }
        CreateGrid();
        SetGridNumber();
        Debug.Log("SudokuGrid End");
    }

    // void Update()
    // {
        
    // }

    private void CreateGrid()
    {
        Debug.Log("CreateGrid Start");
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        Debug.Log("SpawnGridSquares Start");
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {   
                gridSquareList.Add(Instantiate(gridSquare) as GameObject);
                gridSquareList[gridSquareList.Count - 1].transform.parent = this.transform;
                gridSquareList[gridSquareList.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
            }
        }
    }

    private void SetSquaresPosition()
    {
        Debug.Log("SetSquaresPosition Start");
        var squareRect = gridSquareList[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + squareOffset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + squareOffset;

        int columnNumber = 0;
        int rowNumber = 0;

        foreach (GameObject square in gridSquareList)
        {
            if (columnNumber + 1 > columns)
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

    private void SetGridNumber()
    {
        Debug.Log("SetGridNumber Start");
        foreach (var square in gridSquareList)
        {
            square.GetComponent<GridSquare>().SetNumber(UnityEngine.Random.Range(1, 10));
        }
    }
}
