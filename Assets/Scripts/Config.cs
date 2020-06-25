using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;



public class Config : MonoBehaviour
{
    #if UNITY_ANDROID && !UNITY_EDITOR
        private static string dir = Application.persistentDataPath;
    #else
        private static string dir = Directory.GetCurrentDirectory();
    #endif

    private static string file = @"\BoardData.ini";

    private static string path = dir + file;

    public static void DeleteDataFile()
    {
        File.Delete(path);
    }

    public static void SaveBoardData(SudokuData.SudokuBoardData boardData, string level, int boardIndex, 
                                        int errorNumber, Dictionary<string, List<string>> gridNotes)
    {
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);

        string currentTime = "#time:" + PlayTime.instance.TimerTime; //? 9:40
        string levelString = "#level:" + level;
        string errorNumberString = "#errors:" + errorNumber;
        string boardIndexString = "#boardIndex:" + boardIndex.ToString();
        string unsolvedString = "#unsolved:";
        string solvedString = "#solved:";

        foreach (var unsolvedData in boardData.unsolvedData)
        {
            unsolvedString += unsolvedData.ToString() + ",";
        }

        foreach (var solvedData in boardData.solvedData)
        {
            solvedString += solvedData.ToString() + ",";
        }

        writer.WriteLine(currentTime);
        writer.WriteLine(levelString);
        writer.WriteLine(errorNumberString);
        writer.WriteLine(boardIndexString);
        writer.WriteLine(unsolvedString);
        writer.WriteLine(solvedString);

        foreach (var square in gridNotes)
        {
            string squareString = "#" + square.Key + ":";
            bool save = false;

            foreach (var note in square.Value)
            {
                if (note != " ")
                {
                    squareString += note + ",";
                    save = true;
                }
            }

            if (save)
            {
                writer.WriteLine(squareString);
            }
        }

        writer.Close();
    }

    public static Dictionary<int, List<int>> GetGridNotes()
    {
        Dictionary<int, List<int>> gridNotes = new Dictionary<int, List<int>>();
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#squareNote")
            {
                int squareIndex = -1;
                List<int> notes = new List<int>();
                int.TryParse(word[1], out squareIndex);

                string[] substring = Regex.Split(word[2], ",");

                foreach (var note in substring)
                {
                    int noteNumber = -1;
                    int.TryParse(note, out noteNumber);
                    if (noteNumber > 0)
                    {
                        notes.Add(noteNumber);
                    }
                }

                gridNotes.Add(squareIndex, notes);
            }
        }

        file.Close();

        return gridNotes;
    }

    public static string ReadBoardLevel()
    {
        string line;
        string level = "";
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#level")
            {
                level = word[1];
            }
        }

        file.Close();

        return level;
    }

    public static SudokuData.SudokuBoardData ReadGridData()
    {
        string line = "";
        StreamReader file = new StreamReader(path);

        int[] unsolvedData = new int[81];
        int[] solvedData = new int[81];

        int unsolvedIndex = 0;
        int solvedIndex = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        unsolvedData[unsolvedIndex] = squareNumber;
                        unsolvedIndex++;
                    }
                }
            }
        }

        file.BaseStream.Position = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        solvedData[solvedIndex] = squareNumber;
                        solvedIndex++;
                    }
                }
            }
        }

        file.Close();

        return new SudokuData.SudokuBoardData(unsolvedData, solvedData);
    }

    public static int ReadGameBoardLevel()
    {
        int level = -1;
        string line = "";
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#boardIndex")
            {
                int.TryParse(word[1], out level);
            }
        }

        file.Close();

        return level;
    }

    public static float ReadGameTime()
    {
        float playTime = -1.0f;
        string line = "";
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');

            if (word[0] == "#time")
            {
                float.TryParse(word[1], out playTime);                
            }
        }

        file.Close();
        
        return playTime;
    }

    public static int ErrorNumber()
    {
        int errors = 0;
        string line = "";
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#errors")
            {
                int.TryParse(word[1], out errors);
            }
        }

        file.Close();

        return errors;
    }

    public static bool IsGameDataFileExist()
    {
        return File.Exists(path);
    }
}
