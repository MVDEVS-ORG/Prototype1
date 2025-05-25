using prototype1.scripts.systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager instance;

    public static event Action<DayStates> OnDayStateChanged;
    public static DayStates CurrentDayState = DayStates.Day;

    public List<Enemy> enemies = new();

    private int NumberOfSpawners = 0;
    private int FinishedSpawns = 0;
    private Coroutine _dayShift;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public static void ChangeDayState(DayStates state)
    {
        Debug.Log(state);
        CurrentDayState = state;
        if (OnDayStateChanged != null)
        {
            OnDayStateChanged.Invoke(state);
        }
    }

    public void ChangeDay(bool day)
    {
        ChangeDayState(day?DayStates.Day:DayStates.Night);
    }

    public void AddEnemySpawners()
    {
        NumberOfSpawners++;
    }

    public void AllEnemiesSpawned()
    {
        FinishedSpawns++;
        if(FinishedSpawns>=NumberOfSpawners)
        {
            if(_dayShift!=null)
            {
                StopCoroutine(_dayShift);
                _dayShift = null;
            }
            _dayShift = StartCoroutine(CheckIfDayShiftPossible());
        }
    }

    private IEnumerator CheckIfDayShiftPossible()
    {
        while(enemies.Count != 0)
        {
            yield return new WaitForEndOfFrame();
        }
        ChangeDayState(DayStates.Day);
    }
}

public enum DayStates
{
    Day,
    Night
}
