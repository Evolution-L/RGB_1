using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMouseMessagerTarget : IEventSystemHandler
{
    void MouseOver();
    void MouseExit();
    void MouseClick();
}
