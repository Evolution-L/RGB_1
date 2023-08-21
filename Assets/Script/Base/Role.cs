using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;

public interface IAge
{
    public int Age { get; }
    public void AddAge();
}

public abstract class Role
{
    public StateMachine stateMachine;
    protected AnimController animController;
    protected Vector2 curDirction = Vector2.zero;

    protected Spine.AnimationState animationState;

    public GameObject gameObject;
    public Transform transform;

    public Vector2 CurDirction { get => curDirction; }

    // Start is called before the first frame update
    public virtual void Init()
    {
        transform = gameObject.transform;

        stateMachine = new();
        animController = new();
        stateMachine.Init();
        animController.Init(this);

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
