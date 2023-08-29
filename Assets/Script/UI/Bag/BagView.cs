using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BagView : MonoBehaviour, IUIPanel
{
    private BagDataProxy dataProxy;
    public List<GameObject> bag1 = new();
    public List<GameObject> bag2 = new();
    public List<GameObject> bag3 = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        dataProxy = GameDataManager.Instance.bagData;

        for (int i = 0; i < 10; i++)
        {

        }
    }

    public void Dispose()
    {
        
    }
}
