using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance {
        get => instance;
    }

    private const int allLayer = 200;
    [SerializeField]
    private Transform uiParent;
    private Dictionary<string, IPanelDataProxy> proxtDic = new();
    private Dictionary<string, IUIPanel> panelDic = new();
    private List<string> uiNameList = new();

    private void Awake()
    {
        instance = this;
        uiParent = transform.Find("Canvas").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("OPOPPOP");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public T GetPanelDataProxy<T>(string name) where T : IPanelDataProxy, new()
    {
        if (proxtDic.TryGetValue(name, out IPanelDataProxy panelDateProxy))
        {
            return panelDateProxy as T;
        }
        else
        {
            T dataProxy = new T();
            proxtDic[name] = dataProxy;
            return dataProxy;
        }

    }

    public void RemovePanelDataProxy<T>(string name) where T : IPanelDataProxy, new()
    {
        if (proxtDic.TryGetValue(name, out IPanelDataProxy panelDateProxy))
        {
            panelDateProxy.Dispose();
            proxtDic.Remove(name);
        }
        else
        {
            Debug.Log("尝试移除不存在的数据代理");
        }

    }

    public void Push(string name)
    {
        if (!panelDic.ContainsKey(name))
        {
            if (UIConfig.uiConfig.TryGetValue(name, out Type t))
            {
                GameObject go = AssetManager.LoadUIPrefab(name);
                go.transform.SetParentEx(uiParent);
                var c = go.AddComponent(t) as IUIPanel;
                c.Init();

                panelDic[name] = c;
                uiNameList.Add(name);
            }
            else
                Debug.Log("尝试添加不存在的UI");
        }
    }

    public void Destroy(string name)
    {
        if (panelDic.TryGetValue(name, out IUIPanel uIPanel))
        {
            uIPanel.Dispose();
            panelDic.Remove(name);
            uiNameList.Remove(name);
        }
        else
            Debug.Log("尝试移除不存在的UI");
    }
}
