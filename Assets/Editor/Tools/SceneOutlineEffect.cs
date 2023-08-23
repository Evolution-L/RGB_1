using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class SceneOutlineEffect
{
    static SceneOutlineEffect()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type != EventType.Repaint)
        {
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (var item in selectedObjects)
        {
            DrawOutline(item);
        }
    }

    private static void DrawOutline(GameObject selectedObject)
    {
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

        Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Bounds bounds = renderer.bounds;
            Handles.DrawWireCube(bounds.center, bounds.size);
        }

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
    }
}
