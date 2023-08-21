using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using CustomEvent;

struct AnimConfig {
    public readonly string name;
    public readonly bool loop;
    public readonly bool breakCur;

    public AnimConfig(string name, bool loop, bool breakCur)
    {
        this.name = name;
        this.loop = loop;
        this.breakCur = breakCur;
    }
}
public class AnimController
{
    SkeletonAnimation animationState;
    Role role;
 
    //private Dictionary<State, List<string>> stateAnim = new Dictionary<State, List<string>>{
    //    { State.Die, new List<string>{"die"} },
    //    { State.Moving, new List<string>{"idle"} },
    //};
    private Dictionary<State, AnimConfig> stateAnim = new Dictionary<State, AnimConfig>{
        { State.Die, new AnimConfig("die", false, true) },
        { State.Moving, new AnimConfig("walk", true, false)},
        { State.Stop, new AnimConfig("idle", true, false)},
    };
    public void Init(Role role, string initName = "idle_down", bool loop = true)
    {
        this.role = role;
        this.animationState = role.gameObject.GetComponent<SkeletonAnimation>();
        this.animationState.state.SetAnimation(0, initName, loop);
        // 订阅动画播放完毕事件
        animationState.state.Complete += OnAnimationComplete;

        //EventManager.Register<EventArgsStateChange>(OnStateChangeNotify);
    }

    public void Update()
    {

    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        var animName = trackEntry.Animation.Name;

        foreach (var state in stateAnim.Keys)
        {
            if (role.stateMachine.CheckState(state))
            {
                PlayAnim(stateAnim[state]);
                break;
            }
        }
    }

    //PlayAnim
    private void PlayAnim(AnimConfig animConfig) {
        var dir = "_down";
        if (role.CurDirction == Vector2.left || role.CurDirction == new Vector2(-1,1) || role.CurDirction == new Vector2(-1, -1))
        {
            dir = "_left";
        }
        else if (role.CurDirction == Vector2.right || role.CurDirction == Vector2.one || role.CurDirction == new Vector2(1, -1))
        {
            dir = "_right";
        }
        else if (role.CurDirction == Vector2.up)
        {
            dir = "_up";
        }
        else if (role.CurDirction == Vector2.down)
        {
            dir = "_down";
        }
        var anim = animConfig.name + dir;
        if (!(GetCurAnim() + dir == anim))
        {
            animationState.state.SetAnimation(0, anim, animConfig.loop);
        } 
    }

    public void OnStateChangeNotify()
    {
        bool is_play = false;
        foreach (var state in stateAnim.Keys)
        {
            if (role.stateMachine.CheckState(state))
            {
                PlayAnim(stateAnim[state]);
                is_play = true;
                break;
            }
        }

        if (!is_play)
        {
            PlayAnim(stateAnim[State.Stop]);
        }
    }



    public string GetCurAnim() 
    {
        return animationState.state.GetCurrent(0).Animation.Name;
    }

    public void OnDestroy()
    {

    }
}
