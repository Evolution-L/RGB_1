using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;



public partial class Player : Role
{
    public State PlayerState
    {
        get { return playerState; }
    }

    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        this.playerState |= State.Alive;
        inputListener = new InputListener();
        inputListener.Init(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (stateMachine.CheckState(State.Moving))
        {
            Move();
        }

        inputListener.Update();
        stateMachine.Update();
    }

    public void Move()
    {
        if (nextTargetPosition != transform.position)
        {
            var distance = Vector3.Distance(transform.position, nextTargetPosition);
            if (distance < 0.01f)
            {
                
                transform.position = nextTargetPosition;
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, nextTargetPosition, Define.SPEED * Time.deltaTime);
        }
    }

    public void SetNextPosition(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Vector3 pos = dir * Define.STEP;
            var temp = Vector2ToDir(dir);
            if (curDirction != temp)
            {
                animController.OnStateChangeNotify();
            }
            curDirction = temp;
            nextTargetPosition = transform.position + pos;
            
            stateMachine.ChangeState(State.Moving);
        }
        else
        {
            stateMachine.CancelState(State.Moving);
        }
    }

    private int Vector2ToDir(Vector2 v2)
    {
        if (v2.x == 0 && v2.y == 1)
        {
            return 0;
        }
        else if (v2.x == 1 && v2.y == 1)
        {
            return 1;
        }
        else if (v2.x == 1 && v2.y == 0)
        {
            return 2;
        }
        else if (v2.x == 1 && v2.y == -1)
        {
            return 3;
        }
        else if (v2.x == 0 && v2.y == -1)
        {
            return 4;
        }
        else if (v2.x == -1 && v2.y == -1)
        {
            return 5;
        }
        else if (v2.x == -1 && v2.y == 0)
        {
            return 6;
        }
        else if (v2.x == -1 && v2.y == -1)
        {
            return 7;
        }

        return 4;
    }

    private bool CheckState(State state)
    {
        return stateMachine.CheckState(state);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void PlayAnim(int args)
    {
        
    }

    public void MouseOver()
    {
        Debug.Log("AAAAAA  ");
    }

    public void MouseExit()
    {
    }

    public void MouseClick()
    {
        Debug.Log("AAAAAA  ");
    }
}
