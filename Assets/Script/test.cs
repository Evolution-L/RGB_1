using System.Diagnostics;
using UnityEngine;
using CustomEvent;
using System;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Collections.Generic;

[Flags]
enum aa
{
    a = 1 << 0,
    b = 1 << 1,
    c = 1 << 2,
    d = b | c
}
public class test : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.onMinuteChange += bbb;
        bbb();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void bbb()
    {
        gameObject.GetComponentInChildren<Text>().text = EnvironmentManager.Instance.GetSeasonString() + " " + TimeManager.Instance.ToString();
    }

}