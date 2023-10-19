using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test001))]
public class Test001Editor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();



        Test001 myScript = (Test001)target;

        if (GUILayout.Button("InitGame"))
        {

            myScript.test1();

        }
        if (GUILayout.Button("EndGame"))
        {

            myScript.test2();

        }        
        if (GUILayout.Button("���԰�ť"))
        {

            myScript.test3();

        }        
        if (GUILayout.Button("����ֲ��"))
        {

            myScript.test4();

        }

    }
}

[CustomEditor(typeof(Test002))]
public class Test002Editor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();



        Test002 myScript = (Test002)target;

        if (GUILayout.Button("���԰�ť"))
        {

            myScript.test1();

        }        
        if (GUILayout.Button("���԰�ť"))
        {

            myScript.test2();

        }

    }
}