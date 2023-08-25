using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UIInfo
{
    public readonly Type type;
    public readonly bool dataProxy;

    public UIInfo(Type type, bool dataProxy)
    {
        this.type = type;
        this.dataProxy = dataProxy;
    }
}

public static class UIConfig
{
    public static readonly Dictionary<string, UIInfo> uiConfig = new()
    {
        { "Main", new UIInfo(typeof(MainView), false)  },
        { "Bag", new UIInfo(typeof(BagView), true)  },
    };
}
