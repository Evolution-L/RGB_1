using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using System;
using MoleMole;

public class MainViewContext : BaseContext
{
    public string curSeason;
    public string curHp;
    public string curMp;
    public string MaxHp;
    public string MaxMp;

    
    public MainViewContext() : base(UIType.MainView)
    {
    }
}

public class MainView : AnimateView
{
    public Text dateText;
    public Text yearText;
    public Text seasonText;
    public Button bagBtn;

    public Image hp;
    public Image mp;
    public Text hpValue;
    public Text mpValue;

    // Start is called before the first frame update
    void Start()
    {   
        Init();
    }

    public void Init()
    {
        UpdateDate();
        TimeManager.Instance.onMinuteChange += UpdateDate;
        // gameDataManager = Singleton<GameDataManager>.Instance;

        // OnPlayerHpChange();
        // OnPlayerMpChange();

        // EventManager.AddListener<int, int>(EventDefine.playerHpChange, OnPlayerHpChange);
        // EventManager.AddListener<int, int>(EventDefine.playerMpChange, OnPlayerMpChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDate()
    {
        dateText.text = TimeManager.Instance.GetMonthString() + "\n" + TimeManager.Instance.GetTimeString();
        yearText.text = TimeManager.Instance.GetYearString();
        // seasonText.text = EnvironmentManager.Instance.GetSeasonString();
    }

    private void OnPlayerHpChange(int value, int maxVlaue)
    {
        hpValue.text = value.ToString();
        hp.fillAmount = (float)value / maxVlaue;
    }
    private void OnPlayerMpChange(int value, int maxVlaue)
    {
        mpValue.text = value.ToString();
        mp.fillAmount = (float)value / maxVlaue;
    }


    public void Dispose()
    {
        // EventManager.RemoveListener<int, int>(EventDefine.playerHpChange, OnPlayerHpChange);
        // EventManager.RemoveListener<int, int>(EventDefine.playerMpChange, OnPlayerMpChange);
    }
}
