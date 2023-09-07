using System.Collections;
using System.Collections.Generic;
using MoleMole;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private GameObject OutPutCamera;
    public GameObject sceneCamera;
    private GameObject uiCamera;

    public GameObject gameRoot;
    private void Awake()
    {
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
        DontDestroyOnLoad(gameRoot);

        Singleton<GameDataManager>.Create();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGame(string saveFileName)
    {
        Debug.Log(TimeManager.Instance.ToString());
        
        Singleton<GameDataManager>.Instance.LoadGameArchive(saveFileName);
        InitScene();
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
}
