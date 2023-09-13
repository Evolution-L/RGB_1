using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Test001 : MonoBehaviour
{
    public string aaa;
    public void test1()
    {
        // GameManager.Instance.InitGame("test001");
    }
    public void test2()
    {
        // GameManager.Instance.EndGame();
    }    
    public void test3()
    {
        gameObject.SendMessage("Send");
    }    
    public void test4()
    {
        // GrowableManager.Instance.CreateGrowable("1001");
    }
}
