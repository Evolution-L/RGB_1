using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;

public class GameInputManager : MonoBehaviour
{
    List<KeyCode> needListenerList = new List<KeyCode>{
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Q,
        KeyCode.E,
        KeyCode.A,
        KeyCode.W,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
    };
    private Dictionary<KeyCode, float> longPressTime;
    // Start is called before the first frame update
    void Start()
    {
        longPressTime = new();
    }

    void OnDestroy()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in needListenerList)
        {
            if (Input.GetKeyDown(item))
            {
                EventManager.Broadcast(EventDefine.KeyDown, item);
            }

            if (Input.GetKeyUp(item))
            {
                EventManager.Broadcast(EventDefine.KeyUp, item);
            }
        }
    }
}
