using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using System;

public class MainView : MonoBehaviour, IUIPanel
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
        
    }

    public void Init()
    {
        UpdateDate();
        TimeManager.Instance.onMinuteChange += UpdateDate;

        OnPlayerHpChange(GameDataManager.Instance.playerData.CurHp, GameDataManager.Instance.playerData.MaxHp);
        OnPlayerMpChange(GameDataManager.Instance.playerData.CurMp, GameDataManager.Instance.playerData.MaxMp);


        EventManager.AddListener<int, int>(EventDefine.playerHpChange, OnPlayerHpChange);
        EventManager.AddListener<int, int>(EventDefine.playerMpChange, OnPlayerMpChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDate()
    {
        dateText.text = TimeManager.Instance.GetMonthString() + "\n" + TimeManager.Instance.GetTimeString();
        yearText.text = TimeManager.Instance.GetYearString();
        seasonText.text = EnvironmentManager.Instance.GetSeasonString();
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
        EventManager.RemoveListener<int, int>(EventDefine.playerHpChange, OnPlayerHpChange);
        EventManager.RemoveListener<int, int>(EventDefine.playerMpChange, OnPlayerMpChange);
    }
}
