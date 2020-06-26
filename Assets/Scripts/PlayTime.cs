using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTime : MonoBehaviour
{

    private float timerTime;
    public float TimerTime
    {
        get
        {
            return timerTime;
        }
        set
        {
            this.timerTime = value;
        }
    }

    private Text textTimer;
    public Text TextTimer
    {
        get
        {
            return textTimer;
        }
        set
        {
            this.textTimer = value;
        }
    }

    private float deltaTime;

    private bool stopTimer = false;

    public static PlayTime Instance;

    void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance = this; // check InscopeStudio

        TextTimer = GetComponent<Text>(); //? maybe in Start

        if (GameSettings.Instance.ContinuePreviousGame)
        {
            deltaTime = Config.ReadGameTime();
        }
        else
        {
            deltaTime = 0;
        }
    }

    void Start()
    {
        stopTimer = false;
    }

    void Update()
    {
        if (!GameSettings.Instance.Paused && !stopTimer)
        {
            deltaTime += Time.deltaTime;
            TimerTime = deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(deltaTime);

            string hour = LeadingZero(span.Hours);
            string min = LeadingZero(span.Minutes);
            string sec = LeadingZero(span.Seconds);

            TextTimer.text = hour + ":" + min + ":" + sec;
        }
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void OnGameOver()
    {
        stopTimer = true;
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }

    public void StartTimer()
    {
        stopTimer = false;
    }
}
