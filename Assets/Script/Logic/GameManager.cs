using CustomEvent;
using MoleMole;
using UnityEngine;
using QZSXFrameWork.Tools;
// using Asset = QZSXFrameWork.Asset.Asset;
using QZSXFrameWork.Asset;
using QZSXFrameWork;

public class GameManager : MonoSingleton<GameManager>
{
    private GameObject OutPutCamera;
    public GameObject sceneCamera;
    private GameObject uiCamera;

    public GameObject gameRoot;
    private void Awake()
    {
        GameObject go = new GameObject();
        // Singleton<AssetManager>.Create(go.AddComponent<AssetManager>());
        go.name = "AssetManager";
        DontDestroyOnLoad(go);
        // Debug.Log(Application.persistentDataPath);
        OutPutCamera = GameObject.FindGameObjectWithTag("OutPutCamera");
        if (OutPutCamera == null)
        {
            OutPutCamera = GameObject.Instantiate(Asset.GetPrefab("camera/OutPutCamera.prefab"));
        }

        uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        if (uiCamera == null)
        {
            uiCamera = GameObject.Instantiate(Asset.GetPrefab("camera/UICamera.prefab"));
        }


        OutPutCamera.GetComponent<MainCamera>().ClearCameraStack();
        OutPutCamera.GetComponent<MainCamera>().AddCameraToStack(uiCamera.GetComponent<Camera>());

        //ConfigLoader.Instance.Loader();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(OutPutCamera);
        //DontDestroyOnLoad(sceneCamera);
        DontDestroyOnLoad(uiCamera);


        Singleton<GameDataManager>.Create();
        gameRoot = GameObject.Instantiate(Asset.GetPrefab("ui/GameRoot.prefab") as GameObject);
        // gameRoot = Asset.GetPrefab("ui/GameRoot.prefab");

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
        D.Log(TimeManager.Instance.ToString());
        Singleton<GameDataManager>.Instance.LoadGameArchive(gameStartEventArgs.saveFileName);

        InitScene();
        Singleton<ContextManager>.Instance.Push(new MainViewContext());
        TimeManager.Instance.isStart = true;
    }

    public void InitScene()
    {
        //     sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //     sceneCamera = sceneCamera == null ? Asset.GetInsObj("camera/SceneCamera.prefab") : sceneCamera;
        //     OutPutCamera.GetComponent<MainCamera>().ClearCameraStack();
        //     OutPutCamera.GetComponent<MainCamera>().AddCameraToStack(sceneCamera.GetComponent<Camera>());
        //     OutPutCamera.GetComponent<MainCamera>().AddCameraToStack(uiCamera.GetComponent<Camera>());


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
