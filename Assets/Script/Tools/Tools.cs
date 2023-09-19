using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
   public static bool IsDifferenceLessThan(float value1, float value2, float maxDifference)
   {
       float difference = Mathf.Abs(value1 - value2);
       return difference < maxDifference;
   }

    public static Vector3 GridPosToWorldPos(Vector3 gridPos)
    {
        Vector3 pos = default;
        pos.x = (gridPos.x - 2) / 4;
        pos.y = (gridPos.y - 1.5f) / 3;

        return pos;
    }    
    public static Vector3 WorldPosToGridPos(Vector3 worldPos)
    {
        Vector3 pos = default;
        pos.x = worldPos.x * 4 + 2 ;
        pos.y = worldPos.y * 3 + 1.5f;

        return pos;
    }

    public static void PrintLog()
    {
        Debug.Log("程序集加载成功!!!!");
    }

}
