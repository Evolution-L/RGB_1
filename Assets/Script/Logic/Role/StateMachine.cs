using CustomEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����״̬: ʹ�ð�λ�������|��������ĳ��״̬λ�����磬�������һ����ʾ��ɫ�Ƿ������ƶ��ı�־λ�������������ã�playerState |= Moving;
// ȡ��״̬�� ʹ�ð�λ������Ͱ�λȡ��������& �� ~����ȡ��ĳ��״̬λ�����磬ȡ���ƶ�״̬��playerState &= ~Moving;
// ���״̬�� ʹ�ð�λ�������&�������ĳ��״̬�Ƿ񱻼�����磬����Ƿ�������Ծ��if (playerState & Jumping) { /* ִ����Ծ���� */ }
// �л�״̬�� ʹ�ð�λ��������^�����л�ĳ��״̬λ�����״̬λ�Ѿ�������л�����ȡ��״̬�����״̬λδ������л���������״̬�����磬�л��ƶ�״̬��playerState ^= Moving;

[Flags]
public enum State
{
    Alive = 1 << 0,
    Die = 1 << 1,

    Moving = 1 << 2,
    Stop = 1 << 3, // ��ֹ�Լ��ƶ�

    BeMoving = 1 << 4, // ���ƶ�
    NotBeMove = 1 << 5, // ��ֹ���ƶ�

    NotMove = Stop | NotBeMove, // ��ֹ�κη�ʽ���ƶ�
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
            Debug.Log("��ǰ״̬Ϊ ���� , �����л�״̬");
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
    /// �޸�״̬ʹ�� ActivateState �� CancelState, �˷�����������
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
    // ��������
    public void SwitchStateAlive() 
    {
        state = 0;
        onChangeState?.Invoke();
    }

    public void OnDestroy() 
    {  
        
    }
}
