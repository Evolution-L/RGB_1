using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace QZSXFrameWork.Asset
{
    /// <summary>
    /// 提供调用接口
    /// </summary>
    public class Asset
    {
        //public static Animation LoadAnimations(string path)
        //{
        //    return (Animation)Singleton<AssetManager>.Instance.LoadSync(path);
        //}

        public static RuntimeAnimatorController LoadAnimator(string path)
        {
            RuntimeAnimatorController animator = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                animator = o.mainObject as RuntimeAnimatorController;
            });
            return animator;
        }

        public static Dictionary<string, int> paths = new Dictionary<string, int>();

        //        public static Font LoadFonts(string path)
        //        {
        //            return (Font)Singleton<AssetManager>.Instance.LoadSync(path);
        //        }

        public static Material LoadMaterials(string path)
        {
            path += ".mat";
            Material mat = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                if (o != null)
                    mat = o.mainObject as Material;
                o.Require(mat);
            });

            return mat;
        }

        public static GameObject LoadUIPrefab(string path)
        {
            GameObject go = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                if (o != null)
                    go = o.Instantiate();
            });
            return go;
        }

        public static GameObject GetInsObj(string path)
        {
            //if (paths.ContainsKey(path))
            //{
            //    paths[path]++;
            //}
            //else
            //{
            //    paths[path] = 1;
            //}
            GameObject obj = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                if (o != null)
                    obj = o.Instantiate();
            });
            return obj;
        }

        public static GameObject LoadPrefab(string path)
        {
            GameObject obj = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                if (o != null)
                    obj = o.mainObject as GameObject;
            });
            return obj;
        }

        public static Tile LoadTile(string path)
        {
            Tile tile = null;

            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
             {
                 if (o != null)
                     tile = o.mainObject as Tile;
             });

            return tile;
        }



        //        public static RenderTexture LoadRenderTexture(string path)
        //        {
        //            return (RenderTexture)Singleton<AssetManager>.Instance.LoadSync(path);
        //        }

        //        public static Shader LoadShaders(string path)
        //        {
        //            return (Shader)Singleton<AssetManager>.Instance.LoadSync(path);
        //        }

        public static Sprite LoadSprite(string path, int pixelsPerUnit = 100, float pivot_x = 0, float pivot_y = 0)
        {
            Sprite sp = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                if (o != null)
                {
                    if (o.mainObject && o.mainObject.GetType() == typeof(Texture2D))
                    {
                        Texture2D texture = (Texture2D)o.mainObject;
                        Vector2 pivot = new Vector2(pivot_x, pivot_y);
                        sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
                        o.Require(sp);
                        //sp = o.mainObject as Sprite;
                    }
                    else
                    {
                        sp = o.mainObject as Sprite;
                    }
                }
            });
            return sp;
        }

        public static Sprite LoadSpriteUnload(string path, out AssetInfo abi, int pixelsPerUnit = 100, float pivot_x = 0, float pivot_y = 0)
        {
            Sprite sp = null;
            AssetInfo abi_i = null;
            Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
            {
                if (o != null)
                {
                    if (o.mainObject && o.mainObject.GetType() == typeof(Texture2D))
                    {
                        Texture2D texture = (Texture2D)o.mainObject;
                        Vector2 pivot = new Vector2(pivot_x, pivot_y);
                        sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, pixelsPerUnit);
                        o.Require(sp);
                        abi_i = o;
                        //sp = o.mainObject as Sprite;
                    }
                    else
                    {
                        sp = o.mainObject as Sprite;
                    }
                }
            });
            abi = abi_i;
            return sp;
        }

        //        public static Texture LoadTexture(string path)
        //        {
        //            return (Texture)Singleton<AssetManager>.Instance.LoadSync(path);
        //        }

        //        public static GameObject LoadUIPrefab(string path)
        //        {
        //            return (GameObject)Singleton<AssetManager>.Instance.LoadSync(path);
        //        }

        public static Dictionary<string, Dictionary<string, Sprite>> spriteCache = new();
        public static Dictionary<string, Sprite> LoadMulSpriteSync(string path)
        {
            // 
            string origPath = path;
            if (spriteCache.ContainsKey(origPath))
                return spriteCache[origPath];
            Dictionary<string, Sprite> tb = new();
#if AB_MODE
                    // 想要支持大小写命名格式
                    // path = path.ToLower();
                    //string bundleName = path.Split('.')[1];
                    Sprite[] sp = null;
                    Singleton<AssetManager>.Instance.LoadAssetSync(path, (o) =>
                     {
                         if(o!=null)
                         {
                             o.Retain();
                             sp = Array.ConvertAll(o.bundle.LoadAssetWithSubAssets(o.bundle.GetAllAssetNames()[0]), item =>
                             {
                                 if (item.GetType() == typeof(UnityEngine.Sprite))
                                     return (Sprite)item;
                                 return null;
                             });
                             foreach (var item in sp)
                             {
                                 if (item)
                                 {
                                     tb.Add(item.name, item);
                                 }
                             }
                             spriteCache[origPath] = tb;
                         }
                     });
#elif UNITY_EDITOR
            //path = path.Replace('.', '/');
            Sprite[] sp = null;
            string suffix = path.Substring(path.LastIndexOf('.') + 1);
            path = path.Substring(0, path.LastIndexOf('.'));
            path = suffix + "/" + path;
            sp = Array.ConvertAll(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets/Resource/AssetBundles/" + path + ".png"), item =>
            {
                if (item.GetType() == typeof(UnityEngine.Sprite))
                    return (Sprite)item;
                return null;
            });

            foreach (var item in sp)
            {
                if (item)
                {
                    tb.Add(item.name, item);
                }
            }
            spriteCache[origPath] = tb;
#endif
            return tb;
        }

        //        public static void LoadAnimationsAsync(string path, Action<Animation> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //             {
        //                 complete((Animation)obj);
        //             });
        //        }

        //        public static void LoadAudioAsync(string path, Action<AudioClip> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                complete((AudioClip)obj);
        //            });
        //        }

        //        public static void LoadFontsAsync(string path, Action<Font> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                complete((Font)obj);
        //            });
        //        }

        //        public static void LoadMaterialsAsync(string path, Action<Material> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                complete((Material)obj);
        //            });
        //        }

        public static void LoadPrefabAsync(string path, Action<GameObject> complete)
        {
            Singleton<AssetManager>.Instance.LoadAssetAsync(path, (o) =>
            {
                if (o != null)
                {
                    GameObject obj = o.Instantiate();
                    if (complete != null)
                        complete(obj);
                }
                else
                {
                    Debug.LogError("not find obj" + path);
                }

            });
        }

        //        public static void LoadRenderTextureAsync(string path, Action<RenderTexture> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                complete((RenderTexture)obj);
        //            });
        //        }

        //        public static void LoadShadersAsync(string path, Action<Shader> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                complete((Shader)obj);
        //            });
        //        }

        //public static void LoadSpriteAsync(string path, Action<Sprite> complete)
        //{
        //    Singleton<AssetManager>.Instance.LoadAssetSync(path, (obj) =>
        //    {
        //        Sprite sp = null;
        //        if (obj.GetType() == typeof(Texture2D))
        //        {
        //            Texture2D texture = (Texture2D)obj;
        //            sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        //        }
        //        else
        //        {
        //            sp = obj as Sprite;
        //        }
        //        complete(sp);
        //    });
        //}

        //        public static void LoadTextureAsync(string path, Action<Texture> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                complete((Texture)obj);
        //            });
        //        }

        //        public static void LoadUIPrefabAsync(string path, Action<GameObject> complete)
        //        {
        //            Singleton<AssetManager>.Instance.LoadAsync(path, (obj) =>
        //            {
        //                GameObject go = (GameObject)obj;
        //                complete(go);
        //            });
        //        }
    }
}
