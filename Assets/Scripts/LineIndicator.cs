﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public static LineIndicator Instance;

    private int[,] lineData = new int[9, 9]
    {
        {    0,1,2,     3,4,5,     6,7,8 },
        {  9,10,11,  12,13,14,  15,16,17 },
        { 18,19,20,  21,22,23,  24,25,26 },

        { 27,28,29,  30,31,32,  33,34,35 },
        { 36,37,38,  39,40,41,  42,43,44 },
        { 45,46,47,  48,49,50,  51,52,53 },

        { 54,55,56,  57,58,59,  60,61,62 },
        { 63,64,65,  66,67,68,  69,70,71 },
        { 72,73,74,  75,76,77,  78,79,80 }
    };

    private int[] lineDataFlat = new int[81]
    {
            0,1,2,     3,4,5,     6,7,8,
          9,10,11,  12,13,14,  15,16,17,
         18,19,20,  21,22,23,  24,25,26,

         27,28,29,  30,31,32,  33,34,35,
         36,37,38,  39,40,41,  42,43,44,
         45,46,47,  48,49,50,  51,52,53,

         54,55,56,  57,58,59,  60,61,62,
         63,64,65,  66,67,68,  69,70,71,
         72,73,74,  75,76,77,  78,79,80 
    };

    private int [,] squareData = new int [9, 9]
    {
        {  0, 1, 2,   9,10,11,  18,19,20 },
        {  3, 4, 5,  12,13,14,  21,22,23 },
        {  6, 7, 8,  15,16,17,  24,25,26 },

        { 27,28,29,  36,37,38,  45,46,47 },
        { 30,31,32,  39,40,41,  48,49,50 },
        { 33,34,35,  42,43,44,  51,52,53 },
        
        { 54,55,56,  63,64,65,  72,73,74 },
        { 57,58,59,  66,67,68,  75,76,77 },
        { 60,61,62,  69,70,71,  78,79,80 }
    };

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private (int, int) GetSquarePosition(int squareIndex)
    {
        int rowPos = -1;
        int colPos = -1;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (lineData[row,col] == squareIndex)
                {
                    rowPos = row;
                    colPos = col;
                }
            }
        }
        return (rowPos, colPos);
    }

    public int[] GetHorizontalLine(int squareIndex)
    {
        int[] line = new int [9];
        var squareRowPos = GetSquarePosition(squareIndex).Item1;
        
        for (int index = 0; index < 9; index++)
        {
            line[index] = lineData[squareRowPos, index];
        }
        return line;
    }

    public int[] GetVerticalLine(int squareIndex)
    {
        int[] line = new int [9];
        var squareColPos = GetSquarePosition(squareIndex).Item2;

        for (int index = 0; index < 9; index++)
        {
            line[index] = lineData[index, squareColPos];
        }

        return line;
    }

    public int[] GetSquare(int squareIndex)
    {
        int[] line = new int[9];
        var rowPos = -1;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (squareData[row, col] == squareIndex)
                {
                    rowPos = row;
                }
            }
        }

        for (int index = 0; index < 9; index++)
        {
            line[index] = squareData[rowPos, index];
        }

        return line;
    }

    public int[] GetAllSquaresIndexes()
    {
        return lineDataFlat;
    }
}
