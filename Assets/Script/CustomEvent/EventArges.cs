using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public interface IEventArgs
    {
        string argsType { get => "args"; }
    }

    public class GameStartEventArgs :IEventArgs
    {
        public string saveFileName;
    }

    public class EventArgsStateChange : IEventArgs
    {
    }

    // 按下存档按钮是触发事件
    public class LoadGameEventArgs : IEventArgs
    {
        public string gameName;
    }

    #region 日期发生变化
    public class DateChangeEventArgs : IEventArgs
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
    }
    public class YearDateChangeEventArgs : IEventArgs
    {
        public int year;
    }

    public class MonthDateChangeEventArgs : IEventArgs
    {
        public int month;
    }

    public class DayDateChangeEventArgs : IEventArgs
    {
        public int day;
    }

    public class HourDateChangeEventArgs : IEventArgs
    {
        public int hour;
    }
    public class MinuteDateChangeEventArgs : IEventArgs
    {
        public int minute;
    }
    #endregion

}
