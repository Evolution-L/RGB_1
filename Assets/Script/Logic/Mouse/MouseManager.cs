using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseMessager : MonoSingleton<MouseMessager>
{
    private GameObject prevObject;

    private string curHoldId;

    public string CurHoldId
    {
        get => curHoldId;
    }

    public GameObject PrevObject { get => prevObject; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = getMouseOver();
        if (target)
        {
            prevObject = target;
            ExecuteEvents.Execute<IMouseMessagerTarget>(target, null, (handle, data) => {
                handle.MouseOver();
            });
        }
        else
        {
            if (prevObject)
            {
                ExecuteEvents.Execute<IMouseMessagerTarget>(prevObject, null, (handle, data) => {
                    handle.MouseExit();
                });
                prevObject = null;
            }
        }
    }

    private GameObject getMouseOver()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        }, hits);
        foreach (RaycastResult rr in hits)
        {
            GameObject go = rr.gameObject;
            if (go)
                return go;
        }
        return null;
    }
}
