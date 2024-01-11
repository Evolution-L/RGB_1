using CustomEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 设置状态: 使用按位或操作（|）来设置某个状态位。例如，如果你有一个表示角色是否正在移动的标志位，可以这样设置：playerState |= Moving;
// 取消状态： 使用按位与操作和按位取反操作（& 和 ~）来取消某个状态位。例如，取消移动状态：playerState &= ~Moving;
// 检查状态： 使用按位与操作（&）来检查某个状态是否被激活。例如，检查是否正在跳跃：if (playerState & Jumping) { /* 执行跳跃动作 */ }
// 切换状态： 使用按位异或操作（^）来切换某个状态位。如果状态位已经激活，则切换它将取消状态；如果状态位未激活，则切换它将设置状态。例如，切换移动状态：playerState ^= Moving;

[Flags]
public enum State
{
    Alive = 1 << 0,
    Die = 1 << 1,

    Moving = 1 << 2,
    Stop = 1 << 3, // 禁止自己移动

    BeMoving = 1 << 4, // 被移动
    NotBeMove = 1 << 5, // 禁止被移动

    NotMove = Stop | NotBeMove, // 禁止任何方式的移动
}
public class StateMachine
{
    private State state;

    public delegate void OnStateChange ();
    public event OnStateChange onChangeState;

    public State State { get => state; }

    public void Init(State state = State.Alive)
    {
        this.state = state;
    }

    public void Update()
    {
        UpdateState();
    }

    public void ChangeState(State targetState)
    {
        if (!state.HasFlag(State.Alive))
        {
            Debug.Log("当前状态为 死亡 , 不能切换状态");
            return;
        }
        else if (CheckState(targetState))
        {
            return;
        }
        else if (targetState == State.Moving)
        {
            if (!(CheckState(State.Stop) || CheckState(State.BeMoving)))
            {
                ActivateState(targetState);
            }
        }
        else
            ActivateState(targetState);
    }

    public void ActivateState(State targetState) 
    {
        state |= targetState;
        onChangeState?.Invoke();
    }

    public void CancelState(State targetState)
    {
        if (state.HasFlag(targetState))
        {
            state &= ~targetState;
            onChangeState?.Invoke();
        }
    }

    public bool CheckState(State targetState)
    {
        // return (state & state) == state;
        return state.HasFlag(targetState);
    }

    /// <summary>
    /// 修改状态使用 ActivateState 和 CancelState, 此方法仅测试用
    /// </summary>
    /// <param name="targetState"></param>
    public void SwitchState(State targetState)
    {
        state ^= targetState;
    }

    void UpdateState()
    {
        if (state.HasFlag(State.Stop) || state.HasFlag(State.NotMove))
        {
            CancelState(State.Moving);
        }
    }
    // 死亡方法
    public void SwitchStateAlive() 
    {
        state = 0;
        onChangeState?.Invoke();
    }

    public void OnDestroy() 
    {  
        
    }
}
