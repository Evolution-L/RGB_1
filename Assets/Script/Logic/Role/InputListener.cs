using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;

public class InputListener
{
    public Player player;
    //private float longPress= 0f;

    public void Init(Player player)
    {
        this.player = player;
        //EventManager.Register<EventArgsKeyDown>(OnKeyDown);
        //EventManager.Register<EventArgsKeyUp>(OnKeyUp);
    }


    public void Update()
    {
        if (player != null)
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxisRaw("Horizontal");
            dir.y = Input.GetAxisRaw("Vertical");
            player.SetNextPosition(dir);
        }
        
    }


    private void OnDestroy()
    {
  
    }
}
