using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    public static class TransformExtension
    {
        public static void DestroyChildren(this Transform trans)
        {
            foreach (Transform child in trans)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static Transform AddChildFromPrefab(this Transform trans, Transform prefab, string name = null)
        {
            Transform childTrans = GameObject.Instantiate(prefab) as Transform;
            childTrans.SetParent(trans, false);
            if (name != null)
            {
                childTrans.gameObject.name = name;
            }
            return childTrans;
        }

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
}
