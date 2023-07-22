using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TimeManager : MonoBehaviour {
    private int _currentRun = -1;
    private int _currentTick;

    public int levelTimeSeconds = 20;
    public int maxTimeCycles = 3;
    private int _levelTimeTicks;
    public delegate void OnNewTimeCycle();

    public delegate void OnGameOver();

    public static OnNewTimeCycle NewTimeCycleEvent;
    public static OnGameOver GameOverEvent;

    private void _NewTimeCycle()
    {
        _currentRun++;
        _currentTick = 0;
        
        if (_currentRun > maxTimeCycles)
        {
            GameOverEvent?.Invoke();
            Debug.Log("DEAD");
            return;
        }
        
        NewTimeCycleEvent?.Invoke();
    }

    public int GetCurrentTick()
    {
        return _currentTick;
    }

    public int GetCurrentRun()
    {
        return _currentRun;
    }

    public int GetSecondsRemaining()
    {
        return (int)((_levelTimeTicks - _currentTick) / 50.0);
    }

    void Start()
    {
        _levelTimeTicks = levelTimeSeconds * 50;
        _NewTimeCycle();
    }

    void FixedUpdate()
    {
        if (_currentRun > maxTimeCycles)
        {
            return;
        }
        _currentTick++;
        if (_currentTick >= _levelTimeTicks)
        {
            _NewTimeCycle();
        }
    }

    void Update()
    {
        if (_currentTick < 25)
        {
            return;
        }

        if (Input.GetButton("Fire1") && _currentRun <= maxTimeCycles)
        {
            _NewTimeCycle();
        }
    }
}
