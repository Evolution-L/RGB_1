using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;

public class TimerDataProxy : DataProxy
{
    DateChangeEventArgs dateChangeEventArgs;
    YearDateChangeEventArgs yearDateChangeEventArgs;
    MonthDateChangeEventArgs monthDateChangeEventArgs;
    DayDateChangeEventArgs dayDateChangeEventArgs;
    HourDateChangeEventArgs hourDateChangeEventArgs;
    MinuteDateChangeEventArgs minuteDateChangeEventArgs;

    private int minute = 0;
    private int hour = 0;
    private int day = 1;
    private int month = 1;
    private int year = 1;

    public int Minute
    {
        get => minute;
        set
        {
            EventManager.Dispatch<YearDateChangeEventArgs>(yearDateChangeEventArgs);
        }
    }
    public int Hour
    {
        get => hour;
        set
        {
            EventManager.Dispatch<HourDateChangeEventArgs>(hourDateChangeEventArgs);
        }
    }
    public int Day
    {
        get => day;
        set
        {
            EventManager.Dispatch<DayDateChangeEventArgs>(dayDateChangeEventArgs);
        }
    }
    public int Month
    {
        get => month;
        set
        {
            EventManager.Dispatch<MonthDateChangeEventArgs>(monthDateChangeEventArgs);
        }
    }
    public int Year
    {
        get => year;
        set
        {
            EventManager.Dispatch<YearDateChangeEventArgs>(yearDateChangeEventArgs);
        }
    }

    public void Init(string json)
    {
        LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(json);
        this.year = (int)jsonData["year"];
        this.month = (int)jsonData["month"];
        this.day = (int)jsonData["day"];
        this.hour = (int)jsonData["hour"];
        this.minute = (int)jsonData["minute"];
    }

    public string GetDataJson()
    {
        Dictionary<string, int> keyValuePairs = new()
        {
            { "minute", minute },
            { "hour", hour },
            { "day", day },
            { "month", month },
            { "year", year },
        };

        return LitJson.JsonMapper.ToJson(keyValuePairs);
    }
}
