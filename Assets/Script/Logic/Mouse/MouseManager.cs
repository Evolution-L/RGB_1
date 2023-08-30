using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//public class MouseMessager : MonoSingleton<MouseMessager>
public class MouseManager : MonoBehaviour
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
        if (!Camera.main)
        {
            return;
        }
        //GameObject target = getMouseOver();
        //if (target != prevObject)
        //{
        //    prevObject = target;
        //    ExecuteEvents.Execute<IMouseMessagerTarget>(target, null, (handle, data) => {
        //        handle.MouseOver();
        //    });
        //}
        //else
        //{
        //    if (prevObject)
        //    {
        //        ExecuteEvents.Execute<IMouseMessagerTarget>(prevObject, null, (handle, data) => {
        //            handle.MouseExit();
        //        });
        //        prevObject = null;
        //    }
        //}
        GameObject target = getMouseOver();
        if (target)
        {
            target.GetComponent<IMouseMessagerTarget>()?.MouseClick();
        }
    }

    private GameObject getMouseOver()
    {
        // 获取鼠标位置
        Vector3 mousePosition = Input.mousePosition;


        // 将屏幕坐标转换为世界坐标
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);


        // 发射射线
        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);
        if (hit.collider)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
