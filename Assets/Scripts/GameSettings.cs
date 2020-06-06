using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!.history/*
//i don't like it. need some GameManager, not MainCamera
public class GameSettings : MonoBehaviour
{
    
    public enum EGameMode
    {
        NOT_SET,
        EASY,
        MEDIUM,
        HARD,
        VERY_HARD
    }

    public static GameSettings Instance;

    private void Awake()
    {
        Paused = false;
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private EGameMode _EGameMode;

    private bool paused;
    public bool Paused
    {
        get
        {
            return paused;
        }
        set
        {
            paused = value;
        }
    }

    void Start() 
    {
        _EGameMode = EGameMode.NOT_SET;
    }

    public void SetGameMode(EGameMode mode)
    {
        _EGameMode = mode;
    }

    public void SetGameMode(string mode)
    {
        if (mode == "Easy")
        {
            SetGameMode(EGameMode.EASY);
        }
        else if (mode == "Medium")
        {
            SetGameMode(EGameMode.MEDIUM);
        }
        else if (mode == "Hard")
        {
            SetGameMode(EGameMode.HARD);
        }
        else if (mode == "VeryHard")
        {
            SetGameMode(EGameMode.VERY_HARD);
        }
        else
        {
            SetGameMode(EGameMode.NOT_SET);
        }
    }

    public string GetGameMode()
    {
        switch (_EGameMode)
        {
            case EGameMode.EASY: return "Easy";
            case EGameMode.MEDIUM: return "Medium";
            case EGameMode.HARD: return "Hard";
            case EGameMode.VERY_HARD: return "VeryHard";
        }

        Debug.LogError("ERROR: Game level is not set");
        return " ";
    }

}
