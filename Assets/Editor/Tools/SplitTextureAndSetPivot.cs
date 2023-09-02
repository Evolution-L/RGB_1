// using System.Numerics;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.Animations;
// using UnityEditor.U2D.Sprites;
// using UnityEngine;
// using Vector2 = UnityEngine.Vector2;
// using Vector3 = UnityEngine.Vector3;

// public class SplitTextureAndSetPivot : EditorWindow
// {
//     string myString = "Test";
//     int slicCount = 0;
//     Vector2 startPoint = new Vector2(0.5f, 0);
//     Vector2 endPoint = new Vector2(0, 0);
//     // float offest = 7f;

//     float slope = 0f;
//     float intercept = 0f;

//     bool groupEnabled;

//     // Add menu named "My Window" to the Window menu
//     [MenuItem("Assets/地编相关工具/TextureTools/切割图片并设置锚点")]
//     static void Init()
//     {
//         // Get existing open window or if none, make a new one:
//         SplitTextureAndSetPivot window = (SplitTextureAndSetPivot)EditorWindow.GetWindow(typeof(SplitTextureAndSetPivot));
//         window.Show();
//     }

//     void OnGUI()
//     {
//         GUILayout.Label("分割texture", EditorStyles.boldLabel);
//         // slicCount = EditorGUILayout.IntField("切片个数", slicCount);
//         startPoint.y = EditorGUILayout.FloatField("startPivot.Y", startPoint.y);
//         endPoint.y = EditorGUILayout.FloatField("endPivot.Y", endPoint.y);
//         // offest = EditorGUILayout.FloatField("offest", offest);

//         // EditorGUILayout.EndToggleGroup();


//         // float interval = (endPoint.y - startPoint.y) / slicCount;

//         // if (EditorGUILayout.LinkButton("Link Button"))
//         //     Debug.Log("Clicked");

//         if (GUILayout.Button("开始生成")) //创建按钮
//         {
//             if (Selection.objects.Length <= 0)
//             {
//                 Debug.Log("slope " + slope);
//                 Debug.Log("intercept " + intercept);
//                 Debug.LogError("未选中物体!!!");
//                 return;
//             }
//             GenFrameAnim();
//         }        
//         if (GUILayout.Button("根据所选图集生成预制体")) //创建按钮
//         {
//             if (Selection.objects.Length <= 0)
//             {
//                 Debug.Log("slope " + slope);
//                 Debug.Log("intercept " + intercept);
//                 Debug.LogError("未选中物体!!!");
//                 return;
//             }
//             CreatePrefab2();
//         }

//     }

//     void GetFormula()
//     {
//         slope = (endPoint.y - startPoint.y) / (endPoint.x - startPoint.x);
//         intercept = startPoint.y - slope * startPoint.x;
//     }

//     public void GenFrameAnim()
//     {
//         string PrefabPath = @"Assets\Resource\AssetBundles\prefab\role_prefab";

//         SetTextureSlice(Selection.objects);
//         // // 选中资源和其路径
//         // Dictionary<Object, string> o_s_dic = new Dictionary<Object, string>();
//         // // List<string> selePath = new List<string>();
//         // foreach (var item in Selection.objects)
//         // {
//         //     // selePath.Add(AssetDatabase.GetAssetPath(item));
//         //     o_s_dic.Add(item, AssetDatabase.GetAssetPath(item));
//         // }
//         // // 选中资源的路径和其子文件路径
//         // Dictionary<string, List<string>> parent_childs_dic = new Dictionary<string, List<string>>();
//         // foreach (var item in o_s_dic.Keys)
//         // {
//         //     var path = AssetDatabase.GetAssetPath(item);
//         //     var childFile = Directory.GetFileSystemEntries(path).ToList();
//         //     parent_childs_dic.Add(path, childFile);
//         // }

//         // foreach (var key in parent_childs_dic.Keys)
//         // {
//         //     GameObject go = null;
//         //     List<Object> objects = new List<Object>();
//         //     foreach (var path in parent_childs_dic[key])
//         //     {

//         //         if (new DirectoryInfo(path).Extension != ".meta")
//         //         {
//         //             if (go == null)
//         //             {
//         //                 Texture2D childAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
//         //                 go = CreatePrefab(childAsset, new DirectoryInfo(key).Name);
//         //             }
//         //             objects.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
//         //         }

//         //     }
//         //     AnimatorController aaaaa = CreateAnimFromSpriteAtlas(objects.ToArray());
//         //     go.GetComponent<Animator>().runtimeAnimatorController = aaaaa;
//         //     PrefabUtility.SaveAsPrefabAssetAndConnect(go, PrefabPath + "\\" + go.name + ".prefab", InteractionMode.AutomatedAction);
//         //     go = null;

//         // }
//     }
//     public void SetTextureSlice(Object[] objects)
//     {
//         // int slicCount = 12;
//         // float startPoint = 0.06f;
//         // float endPoint = 0.91f;
//         // float interval = (endPoint.x - startPoint.x) / slicCount;

//         foreach (var obj in objects)
//         {
//             //DirectoryInfo dir = new DirectoryInfo(AssetDatabase.GetAssetPath(obj));
//             var path = AssetDatabase.GetAssetPath(obj);
//             //var childFile = Directory.GetFileSystemEntries(path); // 获取子文件路径集合


//             //foreach (var child_path in childFile) // 此处遍历子文件
//             //{
//             //    DirectoryInfo aaaaa = new DirectoryInfo(child_path);
//             //    if (aaaaa.Extension == ".meta")
//             //    {
//             //        continue;
//             //    }

//             #region 设置相关属性
//             TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
//                 textureImporter.textureType = TextureImporterType.Sprite;
//                 textureImporter.spriteImportMode = SpriteImportMode.Multiple;
//                 textureImporter.spritePixelsPerUnit = 10;

//                 textureImporter.sRGBTexture = true;
//                 textureImporter.alphaIsTransparency = true;
//                 textureImporter.isReadable = true;
//                 textureImporter.mipmapEnabled = false;

//                 textureImporter.wrapMode = TextureWrapMode.Clamp;
//                 textureImporter.filterMode = FilterMode.Bilinear;

//                 TextureImporterFormat importerFormat = TextureImporterFormat.ASTC_6x6;

//                 var textureImporter1 = textureImporter.GetPlatformTextureSettings("Android");
//                 textureImporter1.name = "Android";
//                 textureImporter1.format = importerFormat;
//                 textureImporter1.overridden = true;
//                 textureImporter.SetPlatformTextureSettings(textureImporter1);

//                 var textureImporter2 = textureImporter.GetPlatformTextureSettings("iPhone");
//                 textureImporter2.name = "iPhone";
//                 textureImporter2.format = importerFormat;
//                 textureImporter2.overridden = true;
//                 textureImporter.SetPlatformTextureSettings(textureImporter2);

//                 textureImporter.SaveAndReimport();
//                 #endregion

//                 //Object childAsset = AssetDatabase.LoadAssetAtPath<Object>(child_path); // 加载子文件
//                 Object childAsset = obj; // 加载子文件

//                 var factory = new SpriteDataProviderFactories();
//                 factory.Init();
//                 var dataProvider = factory.GetSpriteEditorDataProviderFromObject(childAsset);
//                 dataProvider.InitSpriteEditorDataProvider();

//                 /* Use the data provider */
//                 var spriteRectList = dataProvider.GetSpriteRects().ToList();
//                 spriteRectList.Clear();
//                 Texture2D temp = childAsset as Texture2D;
//                 slicCount = temp.width / 40;
//                 endPoint.x = startPoint.x * slicCount;
//                 GetFormula();

//                 for (var i = 0; i < slicCount; i++)
//                 {
//                     var r = new SpriteRect()
//                     {
//                         name = "A0" + i.ToString(),
//                         spriteID = GUID.Generate(),
//                         rect = new Rect(i * (temp.width / slicCount), 0, temp.width / slicCount, temp.height)
//                     };
//                     float y = slope * ((i + 1) * 0.5f) + intercept;
//                     r.alignment = SpriteAlignment.Custom;
//                     r.pivot = new Vector2(0.5f, y);
//                     spriteRectList.Add(r);
//                 }
//                 // Write the updated data back to the data provider
//                 dataProvider.SetSpriteRects(spriteRectList.ToArray());
//                 // Apply the changes made to the data provider
//                 dataProvider.Apply();
//                 // Reimport the asset to have the changes applied
//                 var assetImporter = dataProvider.targetObject as AssetImporter;
//                 assetImporter.SaveAndReimport();

//                 textureImporter.isReadable = false;
//                 textureImporter.SaveAndReimport();
//             //}
//         }

//         Debug.Log("帧图片设置完成");
//     }

//     public GameObject CreatePrefab(Texture2D t2d, string name)
//     {
//         string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(t2d));
//         path = path + "/" + t2d.name + ".png";
//         TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

//         GameObject go = new GameObject();
//         go.name = new DirectoryInfo(path).Parent.Name;
//         go.AddComponent<Animator>();
//         go.AddComponent<SpriteRenderer>();
//         // go.AddComponent<HeroAvatar>();
//         go.AddComponent<PolygonCollider2D>();

//         string selectionPath = AssetDatabase.GetAssetPath(t2d);
//         Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(selectionPath).OfType<Sprite>().ToArray();

//         var factory = new SpriteDataProviderFactories();
//         factory.Init();
//         var dataProvider = factory.GetSpriteEditorDataProviderFromObject(t2d);
//         dataProvider.InitSpriteEditorDataProvider();
//         var rs = dataProvider.GetSpriteRects();

//         float centerY = 0;
//         if (slicCount % 2 == 0)
//         {
//             Debug.LogError(slicCount / 2);
//             Debug.LogError((slope * (slicCount / 2 * 0.5f + 0.25f) + intercept));
//             centerY = Mathf.Abs((slope * (slicCount / 2 * 0.5f + 0.25f) + intercept) - 0.5f) * t2d.width / 10;
//         }
//         else
//         {
//             Debug.LogError((slicCount - 1) / 2 + 1);
//             Debug.LogError((slope * ((slicCount - 1) / 2 * 0.5f + 0.5f) + intercept));
//             centerY = Mathf.Abs((slope * ((slicCount - 1) / 2 * 0.5f + 0.5f) + intercept) - 0.5f) * t2d.width / 10;
//         }
//         Debug.Log(centerY);

//         for (var i = 0; i < sprites.Length; i++)
//         {
//             var item = sprites[i];
//             GameObject child = new GameObject();
//             child.transform.SetParent(go.transform);
//             child.name = item.name;

//             // child.AddComponent<HeroAvatar>();
//             var sp = child.AddComponent<SpriteRenderer>();
//             sp.sprite = item;
//             var rect = rs[i];
//             var pivot = rect.pivot;
//             float width = t2d.width / 10;
//             float height = t2d.height / 10;
//             float x = (width / sprites.Length) * i;
//             float y = pivot.y * rect.rect.height / 10;
//             x -= (width) / 2;
//             y -= (height) / 2;

//             child.transform.position = new Vector3(x, y + centerY, 0);
//         }
//         Debug.Log("预制体创建完成");
//         return go;

//     }
//     /// <summary>
//     /// 传入精灵数组
//     /// </summary>
//     /// <param name="objects"></param>
//     /// <returns></returns>
//     public AnimatorController CreateAnimFromSpriteAtlas(Object[] objects = null)
//     {
//         // string animationPath = Application.dataPath + @"Assets\Resource\AssetBundles\anim";
//         string animationPath = @"Assets\Resource\AssetBundles\anim";

//         List<Object> objs = new List<Object>();

//         Object[] ppp;

//         if (objects == null)
//         {
//             ppp = Selection.objects;
//         }
//         else
//         {
//             ppp = objects;
//         }

//         foreach (var obj in ppp)
//         {
//             objs.Add(obj);
//         }

//         List<List<Sprite>> sprites = new List<List<Sprite>>();
//         foreach (var item in objs)
//         {
//             var temp = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(item));
//             List<Sprite> sss = new List<Sprite>();
//             foreach (var sip in temp)
//             {
//                 sss.Add(sip as Sprite);
//             }
//             sprites.Add(sss);
//         }

//         AnimationClip clip = new AnimationClip();
//         EditorCurveBinding[] curveBinding = new EditorCurveBinding[40];
//         for (var i = 0; i < curveBinding.Length; i++)
//         {
//             curveBinding[i].type = typeof(SpriteRenderer);
//             curveBinding[i].path = "A0" + i;
//             curveBinding[i].propertyName = "m_Sprite";
//         }

//         ObjectReferenceKeyframe[][] keyFrames = new ObjectReferenceKeyframe[40][];
//         for (int i = 0; i < sprites[0].Count; i++) // 40
//         {
//             keyFrames[i] = new ObjectReferenceKeyframe[objs.Count];
//             for (var j = 0; j < sprites.Count; j++) // 8
//             {
//                 keyFrames[i][j] = new ObjectReferenceKeyframe();
//             }
//         }

//         for (var j = 0; j < sprites[0].Count; j++)
//         {
//             var ff = keyFrames[j];
//             for (var i = 0; i < sprites.Count; i++)
//             {
//                 ff[i].value = sprites[i][j];
//                 ff[i].time = i * 0.1f;
//             }
//         }

//         clip.frameRate = 60;

//         string selectionPath2 = AssetDatabase.GetAssetPath(objs[1]);
//         var rootPath2 = Application.dataPath + selectionPath2.Replace("Assets", "");
//         DirectoryInfo di2 = new DirectoryInfo(rootPath2);
//         System.IO.Directory.CreateDirectory(animationPath + "/" + di2.Parent.Name);
//         AnimationUtility.SetObjectReferenceCurves(clip, curveBinding, keyFrames);
//         AssetDatabase.CreateAsset(clip, animationPath + "\\" + di2.Parent.Name + "\\" + di2.Parent.Name + ".anim");
//         AssetDatabase.SaveAssets();

//         // 创建 animator
//         AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animationPath + "/" + di2.Parent.Name + "/" + di2.Parent.Name + ".controller");
//         AnimatorControllerLayer layer = animatorController.layers[0];
//         AnimatorStateMachine sm = layer.stateMachine;
//         sm.AddState("Empty", new Vector3(450, 0, 0));
//         sm.anyStatePosition = new Vector3(-200, 0);
//         sm.entryPosition = new Vector3(350, 0);
//         int y = 0;

//         AnimatorState state = sm.AddState("default", new Vector3(0, y, 0));
//         AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
//         state.motion = clip as Motion;
//         trans.hasExitTime = false;
//         // trans.canTransitionToSelf = animationMapInfos[clip.name].CanTTSelf == 1 ? true : false;
//         trans.canTransitionToSelf = true;
//         trans.interruptionSource = TransitionInterruptionSource.None;
//         trans.orderedInterruption = true;
//         trans.duration = 0;

//         AssetDatabase.SaveAssets();

//         Debug.Log("动画生成完成");
//         return animatorController;
//     }

//     [MenuItem("Assets/地编相关工具/TextureTools/根据所选图集生成预制体")]
//     public static void CreatePrefab2()
//     {
//         foreach (var se in Selection.objects)
//         {
//             var t2d = se as Texture2D;

//             string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(t2d));
//             path = path + "/" + t2d.name + ".png";
//             TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

//             GameObject go = new GameObject();
//             go.name = new DirectoryInfo(path).Parent.Name;
//             go.AddComponent<Animator>();

//             string selectionPath = AssetDatabase.GetAssetPath(t2d);
//             Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(selectionPath).OfType<Sprite>().ToArray();

//             var factory = new SpriteDataProviderFactories();
//             factory.Init();
//             var dataProvider = factory.GetSpriteEditorDataProviderFromObject(t2d);
//             dataProvider.InitSpriteEditorDataProvider();
//             var rs = dataProvider.GetSpriteRects();

//             bool jishu = sprites.Length % 2 == 1;
//             int mid1 = sprites.Length / 2;
//             int mid2 = mid1;
//             if (jishu)
//             {
//                 mid2 = mid1 + 1;
//             }
//             else
//                 mid2 = mid1 += 1;


//             Vector2 pivot1;
//             Vector2 pivot2;
            

//             for (var i = 0; i < sprites.Length; i++)
//             {
//                 var item = sprites[i];
//                 GameObject child = new GameObject();
//                 child.transform.SetParent(go.transform);
//                 child.name = item.name;

//                 // child.AddComponent<HeroAvatar>();
//                 var sp = child.AddComponent<SpriteRenderer>();
//                 sp.sprite = item;
//                 var rect = rs[i];
//                 var pivot = rect.pivot;

//                 if (mid1 == i)
//                 {
//                     pivot1 = pivot;
//                 }
//                 if (mid2 == i)
//                 {
//                     pivot2 = pivot;
//                 }

//                 float x = (t2d.width / 10 / sprites.Length) * i;
//                 float y = pivot.y * rect.rect.height / 10;
//                 x -= (t2d.width / 10) / 2;
//                 y -= (t2d.height / 10) / 2;
//                 child.transform.position = new Vector3(x, y, 0);
//             }

//             Transform ct1 = go.transform.GetChild(0);
//             Transform ct2 = go.transform.GetChild(go.transform.childCount - 1);
//             var mp = (ct1.position + ct2.position) / 2;


//             for (int i = 0; i < go.transform.childCount; i++)
//             {
//                 Transform ct = go.transform.GetChild(i);
//                 Vector3 vector3 = ct.transform.position;
//                 vector3 -= mp;
//                 ct.transform.position = vector3;
//             }

//             go.name = t2d.name;

//             Debug.Log("预制体创建完成");
//         }
//     }
// }