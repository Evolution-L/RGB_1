using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : MonoBehaviour, IUIPanel
{
    private MainDataProxy mainDataProxy;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("!!!!!!!!");
        mainDataProxy = UIManager.Instance.GetPanelDataProxy<MainDataProxy>("Main");
    }

    public void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dispose()
    {
        UIManager.Instance.RemovePanelDataProxy<MainDataProxy>("Main");
    }
}
