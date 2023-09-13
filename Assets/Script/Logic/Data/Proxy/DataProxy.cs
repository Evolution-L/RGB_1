using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataProxy
{
    public void Init(string json);

    public string GetDataJson();
}