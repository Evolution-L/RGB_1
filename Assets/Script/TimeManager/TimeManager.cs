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

    private int minute = 0;
    private int hour = 0;
    private int day = 1;
    private int month = 1;
    private int year = 1;

    private float second = MAX_SECOND;

    public bool isStart = false;

    public int Minute { get => minute; }
    public int Hour { get => hour; }
    public int Day { get => day; }
    public int Month { get => month; }
    public int Year { get => year; }

    public void Init(string json)
    {
        LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(json);
        this.year = (int)jsonData["year"];
        this.month = (int)jsonData["month"];
        this.day = (int)jsonData["day"];
        this.hour = (int)jsonData["hour"];
        this.minute = (int)jsonData["minute"];
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
            minute += 1;

            if (minute > MAX_MINUTE)
            {
                minute = 0;
                hour += 1;
                if (hour > MAX_HOUR)
                {
                    hour = 0;
                    day += 1;
                    if (day > MAX_DAY)
                    {
                        day = 1;
                        month += 1;
                        if (month > MAX_MONTH)
                        {
                            month = 1;
                            year += 1;
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
        return $"第{Year}年{month:D2}月{day:D2}日\n{hour:D2}:{minute:D2}";
    }

    public string GetYearString()
    {
        return $"第{Year}年";
    }    
    public string GetMonthString()
    {
        return $"{month:D2}月{day:D2}日";
    }    
    public string GetTimeString()
    {
        return $"{hour:D2}:{minute:D2}";
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
