using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrowable : IAge
{
    public string Rid { get; }
    public GrowableCfgItem ConfigId { get; }
    public void SwitchPhase(int next_phase);

};

public class Growable : Role, IGrowable, IMouseMessagerTarget
{
    private string rid;
    private GrowableCfgItem config;
    private int age;
    private int curPhase;
    private bool canHarvest;

    public string Rid => rid;
    public GrowableCfgItem ConfigId => config;
    public int Age => age;
    public int CurPhase => curPhase;
    public bool CanHarvest => canHarvest;

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        stateMachine.ChangeState(State.NotMove);
    }

    public void Init(string rid, GrowableCfgItem config, int age = 0, int curPhase = 1)
    {
        Init();

        this.rid = rid;
        this.config = config;
        this.age = age;
        this.curPhase = curPhase;

        if (curPhase != 1)
        {
            SwitchPhase(curPhase);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void PlayAnim(int phase)
    {
        if (ColorUtility.TryParseHtmlString(config.phaseColor[phase - 1], out Color color))
        {
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void SwitchPhase(int next_phase)
    {
        //var next_phase = curPhase + 1;
        PlayAnim(next_phase);
        curPhase = next_phase;

    }

    public void AddAge()
    {
        age++;
        if (curPhase < config.phaseNum && age > config.phaseCycle[curPhase - 1])
        {
            SwitchPhase(curPhase + 1);
        }
    }

    public void MouseOver()
    {
        Debug.Log("MouseOver");
    }

    public void MouseExit()
    {
        Debug.Log("MouseExit");
    }

    public void MouseClick()
    {
        Debug.Log("!@#!@#!@#!@#");
    }
}
