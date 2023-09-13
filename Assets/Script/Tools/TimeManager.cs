using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeManager : MonoSingleton<TimeManager>
{
    const int MAX_SECOND = 1;
    const int MAX_MINUTE = 60;
    const int MAX_HOUR = 24;
    const int MAX_DAY = 30;
    const int MAX_MONTH = 12;

    TimerDataProxy data;

    public delegate void MinuteChange();
    public event MinuteChange onMinuteChange;
    public delegate void HourChange();
    public event MinuteChange onHourChange;
    public delegate void DayChange();
    public event MinuteChange onDayChange;
    public delegate void MonthChange();
    public event MinuteChange onMonthChange;
    public delegate void YearChange();
    public event MinuteChange onYearChange;

    private float second = MAX_SECOND;

    public bool isStart = false;


    public void Init(string json)
    {

    }

    // Start is called before the first frame update
    void Awake()
    {
        onMinuteChange?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            return;
        }
        second -= Time.deltaTime;
        if (second <= 0)
        {
            second = MAX_SECOND;
            data.Minute += 1;

            if (data.Minute > MAX_MINUTE)
            {
                data.Minute = 0;
                data.Hour += 1;
                if (data.Hour > MAX_HOUR)
                {
                    data.Hour = 0;
                    data.Day += 1;
                    if (data.Day > MAX_DAY)
                    {
                        data.Day = 1;
                        data.Month += 1;
                        if (data.Month > MAX_MONTH)
                        {
                            data.Month = 1;
                            data.Year += 1;
                            onYearChange?.Invoke();
                        }
                        onMonthChange?.Invoke();
                    }
                    onDayChange?.Invoke();
                }
                onHourChange?.Invoke();
            }
            onMinuteChange?.Invoke();
        }
    }

    public override string ToString()
    {
        return $"{data.Year}年{data.Month:D2}月{data.Day:D2}日\n{data.Hour:D2}:{data.Minute:D2}";
    }

    public string GetYearString()
    {
        return $"{data.Year}年";
    }    
    public string GetMonthString()
    {
        return $"{data.Month:D2}月{data.Day:D2}日";
    }    
    public string GetTimeString()
    {
        return $"{data.Hour:D2}:{data.Minute:D2}";
    }
}
