using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    public List<Role> roles;
    private void Awake()
    {
        //ConfigLoader.Instance.Loader();
        roles = new();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in roles)
        {
            item?.Update();
        }
    }

    public void InitScene()
    {
        GameObject go = AssetManager.LoadGameObject("player");
        player = new Player();
        player.gameObject = go;
        player.Init();

        roles.Add(player);
    }
}
