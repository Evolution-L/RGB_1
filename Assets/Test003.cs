using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test003 : MonoBehaviour
{
    public string content;
    public InputField inputField;
    
    // Start is called before the first frame update
    void Start()
    {
        inputField.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Send()
    {
        Debug.Log(content);
    }
}
