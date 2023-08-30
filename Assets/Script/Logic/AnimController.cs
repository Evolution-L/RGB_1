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
public class AnimController : MonoBehaviour
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
    public void Init(string initName = "idle_down", bool loop = true)
    {
        this.role = gameObject.GetComponent<Role>();
        animationState = role.gameObject.GetComponent<SkeletonAnimation>();
        
        if (animationState)
        {
            // 订阅动画播放完毕事件
            animationState.state.Complete += OnAnimationComplete;
            animationState.state.SetAnimation(0, initName, loop);
        }
        
        

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
        var dir = role.CurDirction;

        if (dir == 1 || dir == 2)
        {
            dir = 3;
        }
        if (dir == 7 || dir == 6)
        {
            dir = 5;
        }
        var anim = string.Concat(animConfig.name, "_dir_", dir);
        if (!(GetCurAnim() + dir == anim))
        {
            animationState?.state.SetAnimation(0, anim, animConfig.loop);
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
        return animationState?.state.GetCurrent(0).Animation.Name;
    }

    public void OnDestroy()
    {

    }
}
