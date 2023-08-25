using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    public List<Role> roles;

    private GameObject mainCamera;
    private GameObject sceneCamera;
    private GameObject uiCamera;
    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera = mainCamera == null ? AssetManager.LoadCamera("MainCamera") : mainCamera;
        sceneCamera = GameObject.FindGameObjectWithTag("SceneCamera");
        sceneCamera = sceneCamera == null ? AssetManager.LoadCamera("SceneCamera") : sceneCamera;
        uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        uiCamera = uiCamera == null ? AssetManager.LoadCamera("UICamera") : uiCamera;

        mainCamera.GetComponent<MainCamera>().AddCameraToStack(sceneCamera.GetComponent<Camera>());
        mainCamera.GetComponent<MainCamera>().AddCameraToStack(uiCamera.GetComponent<Camera>());

        uiCamera.AddComponent<UIManager>();

        //ConfigLoader.Instance.Loader();
        roles = new();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(mainCamera);
        //DontDestroyOnLoad(sceneCamera);
        DontDestroyOnLoad(uiCamera);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitScene();
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
        Debug.Log(TimeManager.Instance.ToString());

        GameObject go = AssetManager.LoadGameObject("player");
        player = new Player();
        player.gameObject = go;
        player.Init();

        roles.Add(player);

        sceneCamera.transform.SetParentExt(go.transform);
        sceneCamera.transform.localPosition = new Vector3(0, 0, -10);

        UIManager.Instance.Push("Main");
    }
}
