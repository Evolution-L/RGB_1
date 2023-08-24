using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassExtend
{
    public static void SetParentEx(this Transform child, Transform parent)
    {
        child.SetParent(parent);
        child.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        child.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.SetAsLastSibling();
    }

    public static void SetParentExt(this Transform child, Transform parent)
    {
        child.SetParent(parent);
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;
        child.SetAsLastSibling();
    }

}
