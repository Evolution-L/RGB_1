using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AndroidTest : MonoBehaviour
{
    public Text text;
    AndroidJavaObject _ajc;
    // Start is called before the first frame update
    void Start()
    {
        _ajc = new AndroidJavaObject("com.example.testlib");
        gameObject.GetComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBtnClick()
    {
        bool success = _ajc.Call<bool>("ShowToast", "This is Unity");
        if (success == true)
        {
            Debug.Log("请求成功");
        }
        else
        {
            Debug.Log("call android filed");
        }
    }

    public void FormAndroid(string content)
    {
        text.text = content;
    }
}
