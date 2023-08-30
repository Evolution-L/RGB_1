using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;

public interface IAge
{
    public int Age { get; }
    public void AddAge();
}

public abstract class Role : MonoBehaviour
{
    public StateMachine stateMachine;
    protected AnimController animController;
    protected int curDirction = 4;

    protected Spine.AnimationState animationState;

    public int CurDirction { get => curDirction; }

    // Start is called before the first frame update
    public virtual void Init()
    {

        stateMachine = new();
        animController = new();
        stateMachine.Init();
        animController.Init();

        stateMachine.onChangeState += animController.OnStateChangeNotify;
    }

    public virtual void Update()
    {

    }

    protected virtual void OnDestroy()
    {
        stateMachine.onChangeState -= animController.OnStateChangeNotify;

        stateMachine.OnDestroy();
        animController.OnDestroy();
    }

    public abstract void PlayAnim(int args);

    //public abstract void StateChange();
}
