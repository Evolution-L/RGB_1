using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using System;
using MoleMole;

public class MainViewContext : BaseContext
{   
    public MainViewContext() : base(UIType.MainView)
    {
    }
}

public class MainView : AnimateView
{
    private Text dateText;
    private Text yearText;
    private Text seasonText;
    private Button bagBtn;

    private Image hp;
    private Image mp;
    private Text hpValue;
    private Text mpValue;
    private Button saveButton;

    
    public override void Initialize(BaseContext context)
    {
        BindUI();
        Init();
    }
    

    private void Init()
    {
        
        // gameDataManager = Singleton<GameDataManager>.Instance;
        dateText.text = Singleton<TimerDataProxy>.Instance.GetMonthString() + "\n" + Singleton<TimerDataProxy>.Instance.GetTimeString();
        yearText.text = Singleton<TimerDataProxy>.Instance.GetYearString();

        // OnPlayerHpChange();
        // OnPlayerMpChange();

        // EventManager.AddListener<int, int>(EventDefine.playerHpChange, OnPlayerHpChange);
        EventManager.Register<DateChangeEventArgs>(UpdateDate);
    }

    private void BindUI()
    {
        dateText = GetComponent<Text>("date/date/text");
        yearText = GetComponent<Text>("date/season/text2");
        seasonText = GetComponent<Text>("date/season/text");
        
        hp = GetComponent<Image>("statusBar/hp/hp");
        mp = GetComponent<Image>("statusBar/mp/mp");
        hpValue = GetComponent<Text>("statusBar/hp/value");
        mpValue = GetComponent<Text>("statusBar/mp/value");

        saveButton = GetComponent<Button>("SaveGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateDate(DateChangeEventArgs dateChangeEventArgs)
    {
        dateText.text = Singleton<TimerDataProxy>.Instance.GetMonthString() + "\n" + Singleton<TimerDataProxy>.Instance.GetTimeString();
        yearText.text = Singleton<TimerDataProxy>.Instance.GetYearString();
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
    protected override void OnDestroy() {
                // EventManager.RemoveListener<int, int>(EventDefine.playerHpChange, OnPlayerHpChange);
        // EventManager.RemoveListener<int, int>(EventDefine.playerMpChange, OnPlayerMpChange);
        EventManager.UnRegister<DateChangeEventArgs>(UpdateDate);
    }

}
