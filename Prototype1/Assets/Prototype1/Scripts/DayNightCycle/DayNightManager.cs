using System;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager instance;

    public static event Action<DayStates> OnDayStateChanged;
    public static DayStates CurrentDayState = DayStates.Day;
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
        DayNightManager.ChangeDayState(day?DayStates.Day:DayStates.Night);
    }
}

public enum DayStates
{
    Day,
    Night
}
