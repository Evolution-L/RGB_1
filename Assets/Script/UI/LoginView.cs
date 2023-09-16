using MoleMole;
using UnityEngine;
using CustomEvent;

public class LoginViewContext : BaseContext
{
    public LoginViewContext() : base(UIType.LoginView)
    {
    }
}

public class LoginView : BaseView
{
    private GameObject bg1;
    private GameObject bg2;

    GameStartEventArgs gameStartEventArgs;
    LoadGameEventArgs loadGameEventArgs;

    public override void Initialize(BaseContext context)
    {
        bg1 = GetGameObject("Bg1");
        bg2 = GetGameObject("Bg2");
    }

    // Start is called before the first frame update
    void Start()
    {
        loadGameEventArgs = new();
        gameStartEventArgs = new();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        bg1.SetActive(false);
        bg2.SetActive(true);
    }

    public void SaveBtn1OnClick()
    {
        string fileName = "";
        fileName = Singleton<GameDataManager>.Instance.saveFileNames[0];
        if (!string.IsNullOrEmpty(fileName))
        {
            gameStartEventArgs.saveFileName = fileName;
            Singleton<ContextManager>.Instance.Pop();
            EventManager.Dispatch(gameStartEventArgs);
            // Singleton<ContextManager>.Instance.Push(new MainViewContext());
        }
        else
        {
            // Debug.Log("暂时没有新建功能");
            CreateSaveFile("Save_1");
        }

    }
    public void SaveBtn2OnClick()
    {
        string fileName = "";
        fileName = Singleton<GameDataManager>.Instance.saveFileNames[1];
        if (!string.IsNullOrEmpty(fileName))
        {
            gameStartEventArgs.saveFileName = fileName;
            Singleton<ContextManager>.Instance.Pop();
            EventManager.Dispatch(gameStartEventArgs);
            // Singleton<ContextManager>.Instance.Push(new MainViewContext());
        }
        else
        {
            Debug.Log("暂时没有新建功能");
        }
    }
    public void SaveBtn3OnClick()
    {
        string fileName = "";
        fileName = Singleton<GameDataManager>.Instance.saveFileNames[2];
        if (!string.IsNullOrEmpty(fileName))
        {
            gameStartEventArgs.saveFileName = fileName;
            Singleton<ContextManager>.Instance.Pop();
            EventManager.Dispatch(gameStartEventArgs);
            // Singleton<ContextManager>.Instance.Push(new MainViewContext());
        }
        else
        {
            Debug.Log("暂时没有新建功能");
        }
    }

    public void CreateSaveFile(string name)
    {
        Singleton<GameDataManager>.Instance.CreateSaveFiles(name);
    }

    
    public void Back()
    {
        bg1.SetActive(true);
        bg2.SetActive(false);
    }
}
