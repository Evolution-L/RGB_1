using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private const int allLayer = 200;

    private Transform uiParent;
    private List<GameObject> uiList = new();
    private List<string> uiName = new();

    // Start is called before the first frame update
    void Start()
    {
        uiParent = transform.Find("Canvas").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Push(string name)
    {
        if (!uiName.Contains(name))
        {
            GameObject go = AssetManager.LoadGameObject(name);

        }
    }
}
