using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
}




public class EnvironmentManager : MonoSingleton<EnvironmentManager>
{
    private Season curSeason = Season.Spring;
    public Season CurSeason
    {
        get => curSeason;
    }
    // Start is called before the first frame update
    void Awake()
    {
        TimeManager.Instance.onMonthChange += OnMonthChange;
        TimeManager.Instance.onMinuteChange += OnMinuteChange;
        TimeManager.Instance.onDayChange += OnDayChange;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMonthChange()
    {
        var nextSeason = curSeason;
        if (TimeManager.Instance.Month > 9)
        {
            nextSeason = Season.Spring;
        }
        else if (TimeManager.Instance.Month > 6)
        {
            nextSeason = Season.Summer;
        }
        else if (TimeManager.Instance.Month > 3)
        {
            nextSeason = Season.Autumn;
        }
        else if (TimeManager.Instance.Month > 0)
        {
            nextSeason = Season.Winter;
        }

        if (nextSeason != curSeason)
        {
            curSeason = nextSeason;
            EventManager.Broadcast(EventDefine.SeasonChange);
        }
    }
    public string GetSeasonString()
    {
        if (curSeason == Season.Spring)
        {
            return "´º";
        }
        else if (curSeason == Season.Summer)
        {
            return "ÏÄ";
        }
        else if (curSeason == Season.Autumn)
        {
            return "Çï";
        }
        else if (curSeason == Season.Winter)
        {
            return "¶¬";
        }
        return "";
    }

    void OnMinuteChange()
    {
        //GrowableManager.Instance.UpdateGrowableState();
    }    
    void OnDayChange()
    {
        GrowableManager.Instance.UpdateGrowableState();
    }
}
