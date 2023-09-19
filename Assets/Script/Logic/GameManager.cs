using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using MoleMole;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private GameObject OutPutCamera;
    public GameObject sceneCamera;
    private GameObject uiCamera;

    public GameObject gameRoot;
    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        OutPutCamera = GameObject.FindGameObjectWithTag("OutPutCamera");
        OutPutCamera = OutPutCamera == null ? AssetManager.LoadCamera("OutPutCamera") : OutPutCamera;

        uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        uiCamera = uiCamera == null ? AssetManager.LoadCamera("UICamera") : uiCamera;

        OutPutCamera.GetComponent<MainCamera>().ClearCameraStack();
        OutPutCamera.GetComponent<MainCamera>().AddCameraToStack(uiCamera.GetComponent<Camera>());

        //ConfigLoader.Instance.Loader();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(OutPutCamera);
        //DontDestroyOnLoad(sceneCamera);
        DontDestroyOnLoad(uiCamera);
        

        Singleton<GameDataManager>.Create();
        gameRoot = GameObject.Instantiate(AssetManager.LoadGameObject("GameRoot.prefab") as GameObject);
        DontDestroyOnLoad(gameRoot);
        EventManager.Register<GameStartEventArgs>(InitGame);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGame(GameStartEventArgs gameStartEventArgs)
    {
        Debug.Log(TimeManager.Instance.ToString());
        Singleton<GameDataManager>.Instance.LoadGameArchive(gameStartEventArgs.saveFileName);

        InitScene();
        Singleton<ContextManager>.Instance.Push(new MainViewContext());
        TimeManager.Instance.isStart = true;
    }

    public void InitScene()
    {
        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        sceneCamera = sceneCamera == null ? AssetManager.LoadCamera("SceneCamera") : sceneCamera;
        OutPutCamera.GetComponent<MainCamera>().ClearCameraStack();
        OutPutCamera.GetComponent<MainCamera>().AddCameraToStack(sceneCamera.GetComponent<Camera>());
        OutPutCamera.GetComponent<MainCamera>().AddCameraToStack(uiCamera.GetComponent<Camera>());


        // UIManager.Instance.Push("Main");
    }


    public void EndGame()
    {
        Singleton<GameDataManager>.Instance.SaveData();
    }

    public void OnDestroy()
    {
        EventManager.Register<GameStartEventArgs>(InitGame);
    }
}
