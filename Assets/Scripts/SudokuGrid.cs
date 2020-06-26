using System.Collections;
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

        if (GameSettings.Instance.ContinuePreviousGame)
        {
            SetGridFromFile();
        }
        else
        {
            SetGridNumber(GameSettings.Instance.GetGameMode());
        }

        AdManager.Instance.ShowBanner();
    }

    void SetGridFromFile()
    {
        string level = GameSettings.Instance.GetGameMode();
        selectedGridData = Config.ReadGameBoardLevel();
        var data = Config.ReadGridData();

        SetGridSquareData(data);
        SetGridNotes(Config.GetGridNotes());
    }

    private void SetGridNotes(Dictionary<int, List<int>> notes)
    {
        foreach (var note in notes)
        {
            gridSquareList[note.Key].GetComponent<GridSquare>().SetGridNotes(note.Value);
        }
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
        GameEvents.OnUpdateSquareNumber += CheckBoardCompleted;
    }

    private void OnDisable() 
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnUpdateSquareNumber -= CheckBoardCompleted;

        var solvedData = SudokuData.Instance.sudokuGame[GameSettings.Instance.GetGameMode()][selectedGridData].solvedData;
        int[] unsolvedData = new int[81];
        Dictionary<string, List<string>> gridNotes = new Dictionary<string, List<string>>();

        for (int i = 0; i < gridSquareList.Count; i++)
        {
            var comp = gridSquareList[i].GetComponent<GridSquare>();
            unsolvedData[i] = comp.EnteredNumber;
            string key = "squareNote:" + i.ToString();
            gridNotes.Add(key, comp.GetSquareNotes());
        }

        SudokuData.SudokuBoardData currentGameData = new SudokuData.SudokuBoardData(unsolvedData, solvedData);

        if (!GameSettings.Instance.ExitAfterWon)
        {
            Config.SaveBoardData(currentGameData, GameSettings.Instance.GetGameMode(), selectedGridData, Lives.Instance.ErrorNumber, gridNotes);
        }
        else
        {
            Config.DeleteDataFile();
        }

        AdManager.Instance.HideBanner();
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
        var horizontalLine = LineIndicator.Instance.GetHorizontalLine(squareIndex);
        var verticalLine = LineIndicator.Instance.GetVerticalLine(squareIndex);
        var square = LineIndicator.Instance.GetSquare(squareIndex);

        SetSquaresColor(LineIndicator.Instance.GetAllSquaresIndexes(), Color.white);
        
        SetSquaresColor(horizontalLine, LineHighlightColor);
        SetSquaresColor(verticalLine, LineHighlightColor);
        SetSquaresColor(square, LineHighlightColor);
    }

    private void CheckBoardCompleted(int number)
    {
        foreach (var square in gridSquareList)
        {
            var comp = square.GetComponent<GridSquare>();
            
            if (!comp.IsCorrectNumberSet)
            {
                return;
            }
        }

        GameEvents.BoardCompletedMethod();
    }

    public void SolveSudoku()
    {
        foreach (var square in gridSquareList)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.SetCorrectNumber();
        }

        CheckBoardCompleted(0);
    }
}
