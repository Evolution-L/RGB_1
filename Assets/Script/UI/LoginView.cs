using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using MoleMole;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LoginViewContext : BaseContext
{
    public LoginViewContext() : base(UIType.LoginView)
    {
    }
}

public class LoginView : BaseView
{
    public GameObject bg1;
    public GameObject bg2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        bg1.SetActive(false);
        bg2.SetActive(true);
    }

    public void SaveBtn1OnClick()
    {
        string fileName = "";
        fileName = Singleton<GameDataManager>.Instance.saveFileNames[0];
        if (!string.IsNullOrEmpty(fileName))
        {
            GameManager.Instance.InitGame(fileName);
            Singleton<ContextManager>.Instance.Pop();
            Singleton<ContextManager>.Instance.Push(new MainViewContext());
        }
        else
        {
            Debug.Log("暂时没有新建功能");
        }

    }
    public void SaveBtn2OnClick()
    {
        string fileName = "";
        fileName = Singleton<GameDataManager>.Instance.saveFileNames[1];
        if (!string.IsNullOrEmpty(fileName))
        {
            GameManager.Instance.InitGame(fileName);
            Singleton<ContextManager>.Instance.Pop();
            Singleton<ContextManager>.Instance.Push(new MainViewContext());
        }
        else
        {
            Debug.Log("暂时没有新建功能");
        }
    }
    public void SaveBtn3OnClick()
    {
        string fileName = "";
        fileName = Singleton<GameDataManager>.Instance.saveFileNames[2];
        if (!string.IsNullOrEmpty(fileName))
        {
            GameManager.Instance.InitGame(fileName);
            Singleton<ContextManager>.Instance.Pop();
            Singleton<ContextManager>.Instance.Push(new MainViewContext());
        }
        else
        {
            Debug.Log("暂时没有新建功能");
        }
    }

    
    public void Back()
    {
        bg1.SetActive(true);
        bg2.SetActive(false);
    }
}