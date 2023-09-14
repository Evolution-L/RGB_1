using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using UnityEngine;


public class TimeManager : MonoSingleton<TimeManager>
{
    const int MAX_SECOND = 1;
    const int MAX_MINUTE = 60;
    const int MAX_HOUR = 24;
    const int MAX_DAY = 30;
    const int MAX_MONTH = 12;    
    DateChangeEventArgs dateChangeEventArgs;

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


    public override void Init()
    {
        dateChangeEventArgs = new();
        onMinuteChange?.Invoke();
    }

    // Start is called before the first frame update
    void Awake()
    {
         
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
            var data = Singleton<TimerDataProxy>.Instance;
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
                        }
                    }
                }
            }
            // 在这里主动触发一次时间变化函数, 避免重复触发
            DataChange();
        }
    }

    public void DataChange()
    {
        var data = Singleton<TimerDataProxy>.Instance;

        dateChangeEventArgs.year = data.Year;
        dateChangeEventArgs.month = data.Month;
        dateChangeEventArgs.day = data.Day;
        dateChangeEventArgs.hour = data.Hour;
        dateChangeEventArgs.minute = data.Minute;
        EventManager.Dispatch(dateChangeEventArgs);
    }
}
