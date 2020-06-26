﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> error_images;
    
    int lives_ = 0; //зачем жизни декрементировать, если считаем ошибки. можно же сравнивать кол-во ошибок с кол-вом жизней

    private int errorNumber;
    public int ErrorNumber
    {
        get
        {
            return errorNumber;
        }
        set
        {
            this.errorNumber = value;
        }
    }

    [SerializeField]
    private GameObject gameOverPopUp;

    public static Lives Instance;
    
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

    // Start is called before the first frame update
    void Start()
    {
        lives_ = error_images.Count;
        ErrorNumber = 0;

        if (GameSettings.Instance.ContinuePreviousGame)
        {
            ErrorNumber = Config.ErrorNumber();
            lives_ = error_images.Count - ErrorNumber;

            for (int error = 0; error < ErrorNumber; error++)
            {
                error_images[error].SetActive(true);
            }
        }
    }

    private void WrongNumber()
    {
        if (ErrorNumber < error_images.Count) // i don't like it
        {
            error_images[ErrorNumber].SetActive(true);
            ErrorNumber++;
            lives_--;
        }
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (lives_ <= 0)
        {
            GameEvents.OnGameOverMethod();
            gameOverPopUp.SetActive(true);
        }
    }

    private void OnEnable() 
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    private void OnDisable() 
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }

    public void ResetLives()
    {
        foreach (var error in error_images)
        {
            error.SetActive(false);
        }

        ErrorNumber = 0;
        lives_ = error_images.Count;
    }
}
