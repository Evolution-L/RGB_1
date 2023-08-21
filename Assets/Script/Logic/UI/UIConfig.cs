using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIConfig
{
    public static readonly Dictionary<string, Type> uiConfig = new()
    {
        { "Main", typeof(MainView) },
    };
}
