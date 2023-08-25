using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPanelDataProxy
{
    public abstract void Init();
    public abstract override string ToString();
    public abstract void Dispose();
}
